using System.Data.Common;
using Evently.Common.Infrastructure.Database;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Evently.Modules.Ticketing.Infrastructure.Database;

public sealed class TicketingDbContext(DbContextOptions<TicketingDbContext> options) 
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Ticketing);
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
    
    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is not null)
            await Database.CurrentTransaction.DisposeAsync();

        return (await Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();
    }
}