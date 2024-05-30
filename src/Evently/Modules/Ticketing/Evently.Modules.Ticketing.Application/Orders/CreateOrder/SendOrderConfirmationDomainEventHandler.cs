using Evently.Common.Application.Exceptions;
using Evently.Modules.Ticketing.Application.Orders.GetOrder;
using Evently.Modules.Ticketing.Domain.Orders;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Orders.CreateOrder;

internal sealed class SendOrderConfirmationDomainEventHandler(ISender sender) 
    : DomainEventHandler<OrderCreatedDomainEvent>
{
    public override async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetOrderQuery(domainEvent.OrderId), cancellationToken);
        if (result.IsFailure)
            throw new EventlyException(nameof(GetOrderQuery), result.Error);
        
        // Send order confirmation notification
    }
}