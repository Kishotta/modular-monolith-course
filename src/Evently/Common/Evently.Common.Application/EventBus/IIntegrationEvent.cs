namespace Evently.Common.Application.EventBus;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime OccuredAtUtc { get; }
}

public abstract class IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTime OccuredAtUtc { get; init; }
    
    protected IntegrationEvent(Guid id, DateTime occuredAtUtc)
    {
        Id = id;
        OccuredAtUtc = occuredAtUtc;
    }
    
}