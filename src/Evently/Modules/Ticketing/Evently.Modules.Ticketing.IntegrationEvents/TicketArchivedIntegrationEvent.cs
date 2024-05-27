using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class TicketArchivedIntegrationEvent(
    Guid id,
    DateTime occurredOnUtc,
    Guid ticketId,
    string code)
    : IntegrationEvent(id, occurredOnUtc)
{
    public Guid TicketId { get; init; } = ticketId;
    public string Code { get; init; } = code;
}