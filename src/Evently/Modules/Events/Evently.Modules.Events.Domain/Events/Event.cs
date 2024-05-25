using Evently.Common.Domain.Auditing;
using Evently.Modules.Events.Domain.Categories;

namespace Evently.Modules.Events.Domain.Events;

[Auditable]
public sealed class Event : Entity
{
    public Guid Id { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public DateTime StartsAtUtc { get; private set; }
    public DateTime? EndsAtUtc { get; private set; }
    public EventStatus Status { get; private set; }
    
    private Event() { }

    public static Result<Event> CreateDraft( 
        Category category,
        string title, 
        string description, 
        string location, 
        DateTime startsAtUtc, 
        DateTime? endsAtUtc)
    {
        if (endsAtUtc.HasValue && endsAtUtc < startsAtUtc)
            return Result.Failure<Event>(EventErrors.EndDatePrecedesStartDate);
        
        var @event = new Event
        {
            Id = category.Id,
            CategoryId = category.Id,
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
            Status = EventStatus.Draft
        };

        @event.RaiseDomainEvent(new EventCreatedDomainEvent(@event.Id));
        
        return @event;
    }

    public Result Publish()
    {
        if (Status != EventStatus.Draft)
            return Result.Failure(EventErrors.NotDraft);

        Status = EventStatus.Published;
        
        RaiseDomainEvent(new EventPublishedDomainEvent(Id));

        return Result.Success();
    }
    
    public Result Reschedule(DateTime startsAtUtc, DateTime? endsAtUtc)
    {
        if (endsAtUtc.HasValue && endsAtUtc < startsAtUtc)
            return Result.Failure(EventErrors.EndDatePrecedesStartDate);
        
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
        
        RaiseDomainEvent(new EventRescheduledDomainEvent(Id, startsAtUtc, endsAtUtc));

        return Result.Success();
    }

    public Result Cancel(DateTime utcNow)
    {
        if (Status == EventStatus.Cancelled)
            return Result.Failure(EventErrors.AlreadyCancelled);
        
        if (StartsAtUtc < utcNow)
            return Result.Failure(EventErrors.AlreadyStarted);
        
        Status = EventStatus.Cancelled;
        
        RaiseDomainEvent(new EventCancelledDomainEvent(Id));
        
        return Result.Success();
    }
}