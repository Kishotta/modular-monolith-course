using Evently.Common.Application.EventBus;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.IntegrationEvents;

namespace Evently.Modules.Ticketing.Application.TicketTypes;

internal sealed class TicketTypeSoldOutDomainEventHandler(IEventBus eventBus)
    : IDomainEventHandler<TicketTypeSoldOutDomainEvent>
{
    public async Task Handle(TicketTypeSoldOutDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(
            new TicketTypeSoldOutIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccuredAtUtc,
                domainEvent.TicketTypeId),
            cancellationToken);
    }
}
