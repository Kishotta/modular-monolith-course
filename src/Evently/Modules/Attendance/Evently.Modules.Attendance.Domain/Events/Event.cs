using Evently.Common.Domain;
using Evently.Common.Domain.Auditing;

namespace Evently.Modules.Attendance.Domain.Events;

[Auditable]
public sealed class Event : Entity
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public DateTime StartAtUtc { get; private set; }
    public DateTime? EndAtUtc { get; private set; }

    private Event() {}
    
    public static Event Create(
        Guid id,
        string title,
        string description,
        string location,
        DateTime startAtUtc,
        DateTime? endAtUtc)
    {
        var @event = new Event()
        {
            Id = id,
            Title = title,
            Description = description,
            Location = location,
            StartAtUtc = startAtUtc,
            EndAtUtc = endAtUtc
        };
        
        @event.RaiseDomainEvent(new EventCreatedDomainEvent(
            @event.Id,
            @event.Title,
            @event.Description,
            @event.Location,
            @event.StartAtUtc,
            @event.EndAtUtc));

        return @event;
    }
}