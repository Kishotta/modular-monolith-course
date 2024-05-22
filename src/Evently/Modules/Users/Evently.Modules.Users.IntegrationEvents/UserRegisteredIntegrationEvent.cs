using Evently.Common.Application.EventBus;

namespace Evently.Modules.Users.IntegrationEvents;

public sealed class UserRegisteredIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    
    public UserRegisteredIntegrationEvent(
        Guid id, 
        DateTime occuredAtUtc,
        Guid userId,
        string email,
        string firstName,
        string lastName) 
        : base(id, occuredAtUtc)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}