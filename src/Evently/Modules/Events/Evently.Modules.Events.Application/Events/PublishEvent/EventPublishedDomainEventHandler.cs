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
    : DomainEventHandler<EventPublishedDomainEvent>
{
    public override async Task Handle(EventPublishedDomainEvent notification, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetEventQuery(notification.EventId), cancellationToken);
        if (result.IsFailure)
            throw new EventlyException(nameof(GetEventQuery), result.Error);

        await eventBus.PublishAsync(
            new EventPublishedIntegrationEvent(
                notification.Id,
                notification.OccuredAtUtc,
                result.Value.Id,
                result.Value.Title,
                result.Value.Description,
                result.Value.Location,
                result.Value.StartsAtUtc,
                result.Value.EndsAtUtc,
                result.Value.TicketTypes.Select(ticketType => new TicketTypeModel
                {
                    Id = ticketType.TicketTypeId,
                    EventId = result.Value.Id,
                    Name = ticketType.Name,
                    Price = ticketType.Price,
                    Currency = ticketType.Currency,
                    Quantity = ticketType.Quantity
                }).ToList()), 
            cancellationToken);
    }
}