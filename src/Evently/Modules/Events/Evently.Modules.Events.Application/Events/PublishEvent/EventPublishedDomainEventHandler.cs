using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Events.Application.Events.GetEvent;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Events.Application.Events.PublishEvent;

internal sealed class EventPublishedDomainEventHandler(
    ISender sender,
    IEventBus eventBus)
    : IDomainEventHandler<EventPublishedDomainEvent>
{
    public async Task Handle(EventPublishedDomainEvent notification, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetEventQuery(notification.EventId), cancellationToken);
        if (result.IsFailure)
            throw new EventlyException(nameof(GetEventQuery), result.Error);

        await eventBus.PublishAsync(
            new EventPublishedIntegrationEvent(
                notification.Id,
                notification.OccuredAtUtc,
                result.Value.Id,
                result.Value.Title), 
            cancellationToken);
    }
}