using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class EventTicketsArchivedIntegrationEvent(
    Guid id, 
    DateTime occurredOnUtc,
    Guid eventId)
    : IntegrationEvent(id, occurredOnUtc)
{
    public Guid EventId { get; init; } = eventId;
}