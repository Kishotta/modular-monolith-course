using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.Application.Events.CreateEvent;

namespace Evently.Modules.Ticketing.Presentation.Events;

public sealed class EventPublishedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventPublishedIntegrationEvent>
{
    public override async Task Handle(EventPublishedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new CreateEventCommand(
            integrationEvent.EventId,
            integrationEvent.Title,
            integrationEvent.Description,
            integrationEvent.Location,
            integrationEvent.StartsAtUtc,
            integrationEvent.EndsAtUtc,
            integrationEvent.TicketTypes.Select(ticketType => new CreateEventCommand.TicketType(
                ticketType.Id,
                ticketType.EventId,
                ticketType.Name,
                ticketType.Price,
                ticketType.Currency,
                ticketType.Quantity)
            ).ToList()), cancellationToken);

        if (result.IsFailure)
            throw new EventlyException(nameof(CreateEventCommand), result.Error);
    }
}