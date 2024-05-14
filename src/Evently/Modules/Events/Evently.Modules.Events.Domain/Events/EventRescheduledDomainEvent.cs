namespace Evently.Modules.Events.Domain.Events;

public class EventRescheduledDomainEvent(Guid eventId, DateTime startsAtUtc, DateTime? endsAtUtc) : DomainEvent
{
    public Guid EventId { get; } = eventId;
    public DateTime StartsAtUtc { get; } = startsAtUtc;
    public DateTime? EndsAtUtc { get; } = endsAtUtc;
}