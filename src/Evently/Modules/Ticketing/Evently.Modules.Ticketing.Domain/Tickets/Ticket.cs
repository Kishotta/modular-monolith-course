using Evently.Common.Domain.Auditing;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;

namespace Evently.Modules.Ticketing.Domain.Tickets;

[Auditable]
public sealed class Ticket : Entity
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid EventId { get; private set; }
    public Guid TicketTypeId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public bool Archived { get; private set; }
    
    private Ticket() { }

    public static Ticket Create(Order order, TicketType ticketType)
    {
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            CustomerId = order.CustomerId,
            OrderId = order.Id,
            EventId = ticketType.EventId,
            TicketTypeId = ticketType.Id,
            Code = $"tc_{Ulid.NewUlid()}",
            CreatedAtUtc = DateTime.UtcNow
        };

        ticket.RaiseDomainEvent(new TicketCreatedDomainEvent(ticket.Id));
        
        return ticket;
    }

    public void Archive()
    {
        if (Archived) return;

        Archived = true;
        
        RaiseDomainEvent(new TicketArchivedDomainEvent(Id, Code));
    }
}