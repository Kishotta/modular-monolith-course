using Evently.Common.Application.EventBus;

namespace Evently.Modules.Users.IntegrationEvents;

public sealed class UserNameChangedIntegrationEvent(
    Guid id,
    DateTime occuredAtUtc,
    Guid userId,
    string firstName,
    string lastName)
    : IntegrationEvent(id, occuredAtUtc)
{
    public Guid UserId { get; } = userId;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
}