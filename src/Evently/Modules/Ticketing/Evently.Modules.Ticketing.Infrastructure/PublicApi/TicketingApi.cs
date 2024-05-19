using Evently.Modules.Ticketing.Application.Customers.CreateCustomer;
using Evently.Modules.Ticketing.PublicApi;
using MediatR;

namespace Evently.Modules.Ticketing.Infrastructure.PublicApi;

public class TicketingApi(ISender sender) : ITicketingApi
{
    public async Task CreateCustomerAsync(
        Guid customerId, 
        string email, 
        string firstName, 
        string lastName,
        CancellationToken cancellationToken = default)
    {
        await sender.Send(new CreateCustomerCommand(customerId, email, firstName, lastName), cancellationToken);
    }
}