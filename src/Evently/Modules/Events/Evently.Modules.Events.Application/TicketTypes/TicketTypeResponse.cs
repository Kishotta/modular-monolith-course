using Evently.Modules.Events.Domain.TicketTypes;

namespace Evently.Modules.Events.Application.TicketTypes;

public record TicketTypeResponse(
    Guid Id,
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity)
{
    public static implicit operator TicketTypeResponse(TicketType ticketType) =>
        new TicketTypeResponse(
            ticketType.Id,
            ticketType.EventId,
            ticketType.Name,
            ticketType.Price,
            ticketType.Currency,
            ticketType.Quantity
        );
}