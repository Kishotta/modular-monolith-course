using Evently.Common.Domain;

namespace Evently.Modules.Attendance.Domain.Attendees;

public sealed class AttendeeCheckedInDomainEvent(Guid attendeeId, Guid eventId) : DomainEvent
{
    public Guid AttendeeId { get; } = attendeeId;
    public Guid EventId { get; } = eventId;
}