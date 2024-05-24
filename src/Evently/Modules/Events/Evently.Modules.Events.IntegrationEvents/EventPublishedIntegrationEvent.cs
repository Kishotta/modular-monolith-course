using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public class EventPublishedIntegrationEvent(
    Guid id,
    DateTime occuredAtUtc,
    Guid eventId,
    string title,
    string description,
    string location,
    DateTime startsAtUtc,
    DateTime? endsAtUtc,
    List<TicketTypeModel> ticketTypes)
    : IntegrationEvent(id, occuredAtUtc)
{
    public Guid EventId { get; init; } = eventId;

    public string Title { get; init; } = title;

    public string Description { get; init; } = description;

    public string Location { get; init; } = location;

    public DateTime StartsAtUtc { get; init; } = startsAtUtc;

    public DateTime? EndsAtUtc { get; init; } = endsAtUtc;

    public List<TicketTypeModel> TicketTypes { get; init; } = ticketTypes;
}

public sealed class TicketTypeModel
{
    public Guid Id { get; init; }

    public Guid EventId { get; init; }

    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public string Currency { get; init; } = string.Empty;

    public decimal Quantity { get; init; }
}
