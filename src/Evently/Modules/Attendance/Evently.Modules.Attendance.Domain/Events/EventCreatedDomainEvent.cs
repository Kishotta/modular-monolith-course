using Evently.Common.Domain;

namespace Evently.Modules.Attendance.Domain.Events;

public sealed class EventCreatedDomainEvent(
    Guid eventId,
    string title,
    string description,
    string location,
    DateTime startsAtUtc,
    DateTime? endsAtUtc) : DomainEvent
{
    public Guid EventId { get; } = eventId;
    public string Title { get; } = title;
    public string Description { get; } = description;
    public string Location { get; } = location;
    public DateTime StartsAtUtc { get; } = startsAtUtc;
    public DateTime? EndsAtUtc { get; } = endsAtUtc;
}