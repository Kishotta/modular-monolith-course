using Evently.Common.Application.Exceptions;
using Evently.Modules.Ticketing.Application.Tickets.CreateTicketBatch;
using Evently.Modules.Ticketing.Domain.Orders;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Orders.CreateOrder;

internal sealed class CreateTicketsDomainEventHandler(ISender sender)
    : DomainEventHandler<OrderCreatedDomainEvent>
{
    public override async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CreateTicketBatchCommand(domainEvent.OrderId), cancellationToken);
        if (result.IsFailure)
            throw new EventlyException(nameof(CreateTicketBatchCommand), result.Error);
    }
}