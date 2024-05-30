using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Attendance.Application.Events.CreateEvent;
using Evently.Modules.Events.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Events;

public sealed class EventPublishedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventPublishedIntegrationEvent>
{
    public override async Task Handle(EventPublishedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new CreateEventCommand(
                integrationEvent.EventId,
                integrationEvent.Title,
                integrationEvent.Description,
                integrationEvent.Location,
                integrationEvent.StartsAtUtc,
                integrationEvent.EndsAtUtc),
            cancellationToken);

        if (result.IsFailure)
            throw new EventlyException(nameof(CreateEventCommand), result.Error);
    }
}
