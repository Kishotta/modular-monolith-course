using MassTransit;

namespace Evently.Common.Application.EventBus;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IConsumer<TIntegrationEvent>
where TIntegrationEvent : IntegrationEvent
{
    
}