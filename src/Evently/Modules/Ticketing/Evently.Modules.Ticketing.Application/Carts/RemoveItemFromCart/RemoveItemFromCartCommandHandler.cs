using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Application.Carts.RemoveItemFromCart;

internal sealed class RemoveItemFromCartCommandHandler(
    ICustomerRepository customers,
    ITicketTypeRepository ticketTypes,
    CartService cartService) 
    : ICommandHandler<RemoveItemFromCartCommand>
{
    public async Task<Result> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var customer = await customers.GetAsync(request.CustomerId, cancellationToken);
        if (customer is null)
            return CustomerErrors.NotFound(request.CustomerId);

        var ticketType = await ticketTypes.GetAsync(request.TicketTypeId, cancellationToken);
        if (ticketType is null)
            return TicketTypeErrors.NotFound(request.TicketTypeId);

        await cartService.RemoveItemAsync(customer.Id, ticketType.Id, cancellationToken);

        return Result.Success();
    }
}