namespace Evently.Modules.Events.Domain.Events;

public class EventPublishedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; } = eventId;
}