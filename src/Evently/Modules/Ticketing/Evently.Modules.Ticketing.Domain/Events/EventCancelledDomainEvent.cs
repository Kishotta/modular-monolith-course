namespace Evently.Modules.Ticketing.Domain.Events;

public sealed class EventCancelledDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; } = eventId;
}