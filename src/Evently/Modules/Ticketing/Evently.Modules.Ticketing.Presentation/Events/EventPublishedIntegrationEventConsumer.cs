using Evently.Common.Application.Exceptions;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.Application.Events.CreateEvent;
using MassTransit;

namespace Evently.Modules.Ticketing.Presentation.Events;

public class EventPublishedIntegrationEventConsumer(ISender sender)
    : IConsumer<EventPublishedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<EventPublishedIntegrationEvent> context)
    {
        var result = await sender.Send(new CreateEventCommand(
            context.Message.EventId,
            context.Message.Title,
            context.Message.Description,
            context.Message.Location,
            context.Message.StartsAtUtc,
            context.Message.EndsAtUtc,
            context.Message.TicketTypes.Select(ticketType => new CreateEventCommand.TicketType(
                ticketType.Id,
                ticketType.EventId,
                ticketType.Name,
                ticketType.Price,
                ticketType.Currency,
                ticketType.Quantity)
            ).ToList()));

        if (result.IsFailure)
            throw new EventlyException(nameof(CreateEventCommand), result.Error);
    }
}