using Evently.Common.Application.Exceptions;
using Evently.Modules.Ticketing.Application.Tickets.ArchiveTicketsForEvent;
using Evently.Modules.Ticketing.Domain.Events;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Events.CancelEvent;

internal sealed class ArchiveTicketsEventCanceledDomainEventHandler(ISender sender)
    : DomainEventHandler<EventCancelledDomainEvent>
{
    public override async Task Handle(
        EventCancelledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new ArchiveTicketsForEventCommand(domainEvent.EventId), cancellationToken);
        if (result.IsFailure)
            throw new EventlyException(nameof(ArchiveTicketsForEventCommand), result.Error);
    }
}