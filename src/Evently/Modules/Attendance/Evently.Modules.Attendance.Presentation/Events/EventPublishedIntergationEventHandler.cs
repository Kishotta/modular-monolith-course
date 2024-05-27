using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Attendance.Application.Events.CreateEvent;
using Evently.Modules.Events.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Events;

public sealed class EventPublishedIntegrationEventHandler(ISender sender)
    : IIntegrationEventHandler<EventPublishedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<EventPublishedIntegrationEvent> context)
    {
        var result = await sender.Send(
            new CreateEventCommand(
                context.Message.EventId,
                context.Message.Title,
                context.Message.Description,
                context.Message.Location,
                context.Message.StartsAtUtc,
                context.Message.EndsAtUtc),
            context.CancellationToken);

        if (result.IsFailure)
            throw new EventlyException(nameof(CreateEventCommand), result.Error);
    }
}
