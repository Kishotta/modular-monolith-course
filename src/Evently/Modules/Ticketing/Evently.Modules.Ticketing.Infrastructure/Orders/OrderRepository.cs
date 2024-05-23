using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Orders;

internal sealed class OrderRepository(TicketingDbContext context) : IOrderRepository
{
    public async Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(order => order.OrderItems)
            .SingleOrDefaultAsync(order => order.Id == id, cancellationToken);
    }

    public void Insert(Order order)
    {
        context.Orders.Add(order);
    }
}