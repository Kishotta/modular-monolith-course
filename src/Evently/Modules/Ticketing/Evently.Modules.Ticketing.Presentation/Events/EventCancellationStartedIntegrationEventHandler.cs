using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.Application.Events.CancelEvent;

namespace Evently.Modules.Ticketing.Presentation.Events;

public class EventCancellationStartedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventCancellationStartedIntegrationEvent>
{
    public override async Task Handle(EventCancellationStartedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CancelEventCommand(integrationEvent.EventId), cancellationToken);
        if (result.IsFailure)
            throw new EventlyException(nameof(CancelEventCommand), result.Error);
    }
}