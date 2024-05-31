using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public sealed class EventRescheduledIntegrationEvent(
    Guid id,
    DateTime occurredOnUtc,
    Guid eventId,
    DateTime startsAtUtc,
    DateTime? endsAtUtc)
    : IntegrationEvent(id, occurredOnUtc)
{
    public Guid EventId { get; init; } = eventId;
    public DateTime StartsAtUtc { get; init; } = startsAtUtc;
    public DateTime? EndsAtUtc { get; init; } = endsAtUtc;
}