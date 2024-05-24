using Evently.Common.Domain;

namespace Evently.Modules.Users.Domain.Users;

public class UserNameChangedDomainEvent(
    Guid userId, 
    string firstName,
    string lastName) : DomainEvent
{
    public Guid UserId { get; } = userId;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
}