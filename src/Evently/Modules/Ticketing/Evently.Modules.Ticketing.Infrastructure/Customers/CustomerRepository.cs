using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Customers;

internal sealed class CustomerRepository(TicketingDbContext db) : ICustomerRepository
{
    public async Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken = default) =>
        await db.Customers.SingleOrDefaultAsync(customer => customer.Id == id, cancellationToken);

    public void Insert(Customer customer)
    {
        db.Customers.Add(customer);
    }
}