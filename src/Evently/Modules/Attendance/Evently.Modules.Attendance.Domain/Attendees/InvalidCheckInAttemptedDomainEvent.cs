using Evently.Common.Domain;

namespace Evently.Modules.Attendance.Domain.Attendees;

public sealed class InvalidCheckInAttemptedDomainEvent(
    Guid attendeeId,
    Guid eventId, 
    Guid ticketId, 
    string ticketCode) : DomainEvent
{
    public Guid AttendeeId { get; } = attendeeId;
    public Guid EventId { get; } = eventId;
    public Guid TicketId { get; } = ticketId;
    public string TicketCode { get; } = ticketCode;
}