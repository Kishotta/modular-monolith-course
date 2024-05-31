using Evently.Common.Application.EventBus;
using Evently.Modules.Ticketing.Domain.Tickets;
using Evently.Modules.Ticketing.IntegrationEvents;

namespace Evently.Modules.Ticketing.Application.Tickets.ArchiveTicket;

internal sealed class TicketArchivedDomainEventHandler(IEventBus eventBus) 
    : DomainEventHandler<TicketArchivedDomainEvent>
{
    public override async Task Handle(TicketArchivedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new TicketArchivedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredAtUtc,
                domainEvent.TicketId,
                domainEvent.Code),
            cancellationToken);
    }
}