using Evently.Common.Application.EventBus;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.IntegrationEvents;

namespace Evently.Modules.Users.Application.Users.ChangeUserName;

internal sealed class UserNameChangedDomainEventHandler (
    IEventBus eventBus)
    : IDomainEventHandler<UserNameChangedDomainEvent>
{
    public async Task Handle(UserNameChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(
            new UserNameChangedIntegrationEvent(
                notification.Id,
                notification.OccuredAtUtc,
                notification.UserId,
                notification.FirstName,
                notification.LastName),
            cancellationToken);
    }
}