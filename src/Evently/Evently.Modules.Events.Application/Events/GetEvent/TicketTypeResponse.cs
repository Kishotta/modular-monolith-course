namespace Evently.Modules.Events.Application.Events.GetEvent;

public record TicketTypeResponse(
    Guid TicketTypeId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity);