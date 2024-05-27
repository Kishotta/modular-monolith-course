using Evently.Common.Domain;

namespace Evently.Modules.Attendance.Domain.Tickets;

public sealed class TicketCreatedDomainEvent(Guid ticketId, Guid eventId) : DomainEvent
{
    public Guid TicketId { get; } = ticketId;
    public Guid EventId { get; } = eventId;
}