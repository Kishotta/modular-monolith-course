using System.Data.Common;
using Evently.Common.Infrastructure.Auditing;
using Evently.Common.Infrastructure.Database;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments;
using Evently.Modules.Ticketing.Domain.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Evently.Modules.Ticketing.Infrastructure.Database;

public sealed class TicketingDbContext(DbContextOptions<TicketingDbContext> options) 
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Customer> Customers => Set<Customer>();
    internal DbSet<Event> Events => Set<Event>();
    internal DbSet<TicketType> TicketTypes => Set<TicketType>();
    internal DbSet<Order> Orders => Set<Order>();
    internal DbSet<OrderItem> OrderItems => Set<OrderItem>();
    internal DbSet<Ticket> Tickets => Set<Ticket>();
    internal DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Ticketing);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Common.Infrastructure.AssemblyReference.Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
    
    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is not null)
            await Database.CurrentTransaction.DisposeAsync();

        return (await Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();
    }
}