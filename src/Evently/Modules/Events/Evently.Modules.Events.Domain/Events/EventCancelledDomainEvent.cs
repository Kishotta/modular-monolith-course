namespace Evently.Modules.Events.Domain.Events;

public class EventCancelledDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; } = eventId;
}