namespace Evently.Common.Domain;

public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; init; }
    public DateTime OccurredAtUtc { get; init; }

    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredAtUtc = DateTime.UtcNow;
    }
    
    protected DomainEvent(Guid id, DateTime occurredAtUtc)
    {
        Id = id;
        OccurredAtUtc = occurredAtUtc;
    }
}