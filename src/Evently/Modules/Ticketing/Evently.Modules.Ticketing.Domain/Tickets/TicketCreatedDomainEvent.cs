namespace Evently.Modules.Ticketing.Domain.Tickets;

public sealed class TicketCreatedDomainEvent(Guid ticketId) : DomainEvent
{
    public Guid TicketId { get; } = ticketId;
}