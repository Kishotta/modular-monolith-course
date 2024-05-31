using Evently.Common.Application.Exceptions;
using Evently.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;
using Evently.Modules.Ticketing.Domain.Events;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Events.CancelEvent;

internal sealed class RefundPaymentsEventCanceledDomainEventHandler(ISender sender)
    : DomainEventHandler<EventCancelledDomainEvent>
{
    public override async Task Handle(
        EventCancelledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new RefundPaymentsForEventCommand(domainEvent.EventId), cancellationToken);
        if (result.IsFailure)
            throw new EventlyException(nameof(RefundPaymentsForEventCommand), result.Error);
    }
}