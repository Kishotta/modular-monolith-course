using Evently.Common.Domain;
using Evently.Common.Domain.Auditing;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;

namespace Evently.Modules.Attendance.Domain.Tickets;

[Auditable]
public sealed class Ticket : Entity
{
    public Guid Id { get; private set; }
    public Guid AttendeeId { get; private set; }
    public Guid EventId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public DateTime? UsedAtUtc { get; private set; }

    private Ticket() {}
    
    public static Ticket Create(
        Guid ticketId,
        Attendee attendee,
        Event @event,
        string code)
    {
        var ticket = new Ticket
        {
            Id = ticketId,
            AttendeeId = attendee.Id,
            EventId = @event.Id,
            Code = code
        };
        
        ticket.RaiseDomainEvent(new TicketCreatedDomainEvent(ticket.Id, ticket.EventId));

        return ticket;
    }

    public void MarkAsUsed(DateTime usedAtUtc)
    {
        UsedAtUtc = usedAtUtc;
        RaiseDomainEvent(new TicketUsedDomainEvent(Id));
    }
}