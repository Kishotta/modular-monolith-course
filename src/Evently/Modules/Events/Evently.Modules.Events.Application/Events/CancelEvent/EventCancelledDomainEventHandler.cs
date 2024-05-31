using Evently.Common.Application.EventBus;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.IntegrationEvents;

namespace Evently.Modules.Events.Application.Events.CancelEvent;

internal sealed class EventCancelledDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<EventCancelledDomainEvent>
{
    public override async Task Handle(
        EventCancelledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new EventCancelledIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredAtUtc,
                domainEvent.EventId),
            cancellationToken);
    }
}