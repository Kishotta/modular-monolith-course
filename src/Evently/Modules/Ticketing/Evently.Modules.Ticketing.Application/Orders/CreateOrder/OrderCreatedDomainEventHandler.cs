using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Ticketing.Application.Orders.GetOrder;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Orders.CreateOrder;

internal sealed class OrderCreatedDomainEventHandler(
    ISender sender,
    IEventBus eventBus)
    : DomainEventHandler<OrderCreatedDomainEvent>
{
    public override async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetOrderQuery(domainEvent.OrderId), cancellationToken);
        if (result.IsFailure)
            throw new EventlyException(nameof(GetOrderQuery), result.Error);

        await eventBus.PublishAsync(new OrderCreatedIntegrationEvent(
            domainEvent.Id,
            domainEvent.OccurredAtUtc,
            result.Value.Id,
            result.Value.CustomerId,
            result.Value.TotalPrice,
            result.Value.CreatedAtUtc,
            result.Value.OrderItems.Select(orderItem => new OrderItemModel
            {
                Id = orderItem.OrderItemId,
                OrderId = orderItem.OrderId,
                TicketTypeId = orderItem.TicketTypeId,
                UnitPrice = orderItem.UnitPrice,
                Currency = orderItem.Currency,
                Quantity = orderItem.Quantity,
            }).ToList()), cancellationToken);
    }
}