using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

internal sealed class AddItemToCartCommandHandler(
    ICustomerRepository customers,
    ITicketTypeRepository ticketTypes,
    CartService cartService) : ICommandHandler<AddItemToCartCommand>
{
    public async Task<Result> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        var customer = await customers.GetAsync(command.CustomerId, cancellationToken);
        if (customer is null)
            return Result.Failure(CustomerErrors.NotFound(command.CustomerId));
        
        var ticketType = await ticketTypes.GetAsync(command.TicketTypeId, cancellationToken);
        if (ticketType is null)
            return Result.Failure(TicketTypeErrors.NotFound(command.TicketTypeId));

        var cartItem = new CartItem
        {
            TicketTypeId = ticketType.Id,
            Price = ticketType.Price,
            Quantity = command.Quantity,
            Currency = ticketType.Currency
        };

        await cartService.AddItemAsync(command.CustomerId, cartItem, cancellationToken);

        return Result.Success();
    }
}