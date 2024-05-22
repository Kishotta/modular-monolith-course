using Microsoft.EntityFrameworkCore;

namespace Evently.Common.Infrastructure.Auditing;

public abstract class AuditableDbContext<TDbContext>(DbContextOptions<TDbContext> options) : DbContext(options)
    where TDbContext : DbContext
{
    public DbSet<Audit> AuditLogs => Set<Audit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}