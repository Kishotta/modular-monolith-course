namespace Evently.Common.Application.EventBus;

public abstract class IntegrationEvent(Guid id, DateTime occuredAtUtc) : IIntegrationEvent
{
    public Guid Id { get; init; } = id;
    public DateTime OccuredAtUtc { get; init; } = occuredAtUtc;
}