using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Domain.Events;

public sealed class TicketsArchivedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; } = eventId;
}