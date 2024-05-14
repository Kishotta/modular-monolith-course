namespace Evently.Modules.Events.Domain.Abstractions;

public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; init; }
    public DateTime OccuredAtUtc { get; init; }

    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccuredAtUtc = DateTime.UtcNow;
    }
    
    protected DomainEvent(Guid id, DateTime occuredAtUtc)
    {
        Id = id;
        OccuredAtUtc = occuredAtUtc;
    }
}