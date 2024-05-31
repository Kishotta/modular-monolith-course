namespace Evently.Common.Application.EventBus;

public abstract class IntegrationEvent(Guid id, DateTime occurredAtUtc) : IIntegrationEvent
{
    public Guid Id { get; init; } = id;
    public DateTime OccurredAtUtc { get; init; } = occurredAtUtc;
}