using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public sealed class TicketTypePriceChangedIntegrationEvent(
    Guid id,
    DateTime occurredOnUtc,
    Guid ticketTypeId,
    decimal price)
    : IntegrationEvent(id, occurredOnUtc)
{
    public Guid TicketTypeId { get; init; } = ticketTypeId;
    public decimal Price { get; init; } = price;
}