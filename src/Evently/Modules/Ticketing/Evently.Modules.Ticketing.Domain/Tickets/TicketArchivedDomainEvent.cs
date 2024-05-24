namespace Evently.Modules.Ticketing.Domain.Tickets;

public sealed class TicketArchivedDomainEvent(Guid ticketId) : DomainEvent
{
    public Guid TicketId { get; } = ticketId;
}