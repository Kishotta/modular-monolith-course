using Evently.Common.Domain;

namespace Evently.Modules.Users.Domain.Users;

public class UserRegisteredDomainEvent(Guid userId) : DomainEvent
{
    public Guid UserId { get; } = userId;
}