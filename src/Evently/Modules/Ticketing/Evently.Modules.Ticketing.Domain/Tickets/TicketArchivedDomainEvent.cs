namespace Evently.Modules.Ticketing.Domain.Tickets;

public sealed class TicketArchivedDomainEvent(Guid ticketId, string code) : DomainEvent
{
    public Guid TicketId { get; } = ticketId;
    public string Code { get; } = code;
}