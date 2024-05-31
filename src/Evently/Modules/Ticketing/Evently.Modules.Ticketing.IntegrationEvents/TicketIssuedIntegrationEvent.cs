using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public class TicketIssuedIntegrationEvent(
    Guid id,
    DateTime occurredAtUtc,
    Guid ticketId,
    Guid customerId,
    Guid eventId,
    string code)
    : IntegrationEvent(id, occurredAtUtc)
{
    public Guid TicketId { get; init; } = ticketId;
    public Guid CustomerId { get; init; } = customerId;
    public Guid EventId { get; init; } = eventId;
    public string Code { get; init; } = code;
}