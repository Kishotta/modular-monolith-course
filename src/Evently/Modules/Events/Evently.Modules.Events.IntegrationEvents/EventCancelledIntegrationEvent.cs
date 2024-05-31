using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public sealed class EventCancelledIntegrationEvent(
    Guid id,
    DateTime occurredAtUTc,
    Guid eventId)
    : IntegrationEvent(id, occurredAtUTc)
{
    public Guid EventId { get; init; } = eventId;
}