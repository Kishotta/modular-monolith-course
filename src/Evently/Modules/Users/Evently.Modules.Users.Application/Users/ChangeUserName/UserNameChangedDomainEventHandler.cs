using Evently.Common.Application.EventBus;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.IntegrationEvents;

namespace Evently.Modules.Users.Application.Users.ChangeUserName;

internal sealed class UserNameChangedDomainEventHandler (
    IEventBus eventBus)
    : DomainEventHandler<UserNameChangedDomainEvent>
{
    public override async Task Handle(UserNameChangedDomainEvent notification, CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new UserNameChangedIntegrationEvent(
                notification.Id,
                notification.OccurredAtUtc,
                notification.UserId,
                notification.FirstName,
                notification.LastName),
            cancellationToken);
    }
}