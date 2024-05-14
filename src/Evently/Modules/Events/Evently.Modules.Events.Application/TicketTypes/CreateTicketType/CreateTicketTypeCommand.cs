namespace Evently.Modules.Events.Application.TicketTypes.CreateTicketType;

public record CreateTicketTypeCommand(
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity) : ICommand<TicketTypeResponse>;