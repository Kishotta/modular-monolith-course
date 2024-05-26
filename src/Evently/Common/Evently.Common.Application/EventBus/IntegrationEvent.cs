namespace Evently.Common.Application.EventBus;

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