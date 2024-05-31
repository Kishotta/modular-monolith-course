using Evently.Common.Application.EventBus;

namespace Evently.Modules.Users.IntegrationEvents;

public sealed class UserNameChangedIntegrationEvent(
    Guid id,
    DateTime occurredAtUtc,
    Guid userId,
    string firstName,
    string lastName)
    : IntegrationEvent(id, occurredAtUtc)
{
    public Guid UserId { get; } = userId;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
}