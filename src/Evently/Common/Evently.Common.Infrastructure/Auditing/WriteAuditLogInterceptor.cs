using Evently.Common.Application.Exceptions;
using Evently.Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Evently.Common.Infrastructure.Auditing;

public sealed class WriteAuditLogInterceptor(IAuditingUserProvider auditingUserProvider)
    : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is not null)
            await WriteAuditLog(eventData.Context, cancellationToken);
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

   
    
    private async Task WriteAuditLog(
        DbContext context, 
        CancellationToken cancellationToken)
    {
        var audits = CreateAudits(context);
        
        await context.AddRangeAsync(audits, cancellationToken);
    }
    
    private IEnumerable<Audit> CreateAudits(DbContext context)
    {
        var userId = auditingUserProvider.GetUserId();
        var auditEntries = new List<AuditEntry>();
        
        foreach (var entry in context.ChangeTracker.Entries<Entity>())
        {
            if (!entry.ShouldBeAudited()) continue;
            
            var tableName = context
                .Model
                .FindEntityType(entry.Entity.GetType())
                ?.GetTableName() ?? "Unknown Table";
            
            var auditEntry = new AuditEntry(
                entry,
                tableName,
                userId);
            
            auditEntries.Add(auditEntry);

            foreach (var property in entry.Properties)
            {
                if (!property.IsAuditable()) continue;
                
                var propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditType.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;
                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        auditEntry.AuditType = AuditType.None;
                        break;
                    default:
                        throw new EventlyException(
                            nameof(WriteAuditLog),
                            Error.Failure(
                                "AuditLog.Failure",
                                "Unable to determine entity state for audit log."));
                }
            }
            
        }

        return auditEntries.Select(auditEntry => auditEntry.ToAudit());
    }

    
}