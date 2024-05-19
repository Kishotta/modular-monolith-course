using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Events.PublicApi;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.TicketTypes;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

internal sealed class AddItemToCartCommandHandler(
    CartService cartService, 
    ICustomerRepository customers,
    IEventsApi eventsApi) : ICommandHandler<AddItemToCartCommand>
{
    public async Task<Result> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        var customer = await customers.GetAsync(command.CustomerId, cancellationToken);
        if (customer is null)
            return Result.Failure(CustomerErrors.NotFound(command.CustomerId));
        
        var ticketType = await eventsApi.GetTicketTypeAsync(command.TicketTypeId, cancellationToken);
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