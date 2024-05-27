using Evently.Common.Application.EventBus;
using Evently.Modules.Ticketing.Domain.Tickets;
using Evently.Modules.Ticketing.IntegrationEvents;

namespace Evently.Modules.Ticketing.Application.Tickets.ArchiveTicket;

internal sealed class TicketArchivedDomainEventHandler(IEventBus eventBus) : IDomainEventHandler<TicketArchivedDomainEvent>
{
    public async Task Handle(TicketArchivedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(
            new TicketArchivedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccuredAtUtc,
                domainEvent.TicketId,
                domainEvent.Code),
            cancellationToken);
    }
}