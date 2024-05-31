using Evently.Common.Domain.Auditing;

namespace Evently.Modules.Ticketing.Domain.Events;

[Auditable]
public sealed class Event : Entity
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public DateTime StartsAtUtc { get; private set; }
    public DateTime? EndsAtUtc { get; private set; }
    public bool Cancelled { get; private set; }
    
    private Event() { }
    
    public static Event Create(
        Guid id,
        string title, 
        string description, 
        string location, 
        DateTime startsAtUtc, 
        DateTime? endsAtUtc)
    {
        var @event = new Event
        {
            Id = id,
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc
        };
        
        return @event;
    }
    
    public void Reschedule(DateTime startsAtUtc, DateTime? endsAtUtc)
    {
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
        
        RaiseDomainEvent(new EventRescheduledDomainEvent(Id, startsAtUtc, endsAtUtc));
    }

    public void Cancel()
    {
        if (Cancelled) return;

        Cancelled = true;
        RaiseDomainEvent(new EventCancelledDomainEvent(Id));
    }

    public void PaymentsRefunded()
    {
        RaiseDomainEvent(new EventPaymentsRefundedDomainEvent(Id));
    }

    public void TicketsArchived()
    {
        RaiseDomainEvent(new EventTicketsArchivedDomainEvent(Id));
    }
}