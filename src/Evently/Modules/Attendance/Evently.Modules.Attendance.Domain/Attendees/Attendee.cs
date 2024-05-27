using Evently.Common.Domain;
using Evently.Common.Domain.Auditing;
using Evently.Modules.Attendance.Domain.Tickets;

namespace Evently.Modules.Attendance.Domain.Attendees;

[Auditable]
public sealed class Attendee : Entity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    
    private Attendee() {}
    
    public static Attendee Create(Guid id, string email, string firstName, string lastName)
    {
        return new Attendee
        {
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };
    }

    public void ChangeName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public Result CheckIn(Ticket ticket, DateTime checkInTime)
    {
        if (Id != ticket.AttendeeId)
        {
            RaiseDomainEvent(new InvalidCheckInAttemptedDomainEvent(Id, ticket.EventId, ticket.Id, ticket.Code));
            return Result.Failure(TicketErrors.InvalidCheckIn);
        }

        if (ticket.UsedAtUtc.HasValue)
        {
            RaiseDomainEvent(new InvalidCheckInAttemptedDomainEvent(Id, ticket.EventId, ticket.Id, ticket.Code));
            return Result.Failure(TicketErrors.DuplicateCheckIn);
        }
        
        ticket.MarkAsUsed(checkInTime);
        
        RaiseDomainEvent(new AttendeeCheckedInDomainEvent(Id, ticket.EventId));
        
        return Result.Success();
    }
}