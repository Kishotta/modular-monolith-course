namespace Evently.Modules.Ticketing.Application.Tickets.GetTicketsForOrder;

public sealed record GetTicketsForOrderQuery(Guid OrderId) : IQuery<IReadOnlyCollection<TicketResponse>>;