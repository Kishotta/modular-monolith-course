using Evently.Common.Application.EventBus;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.IntegrationEvents;

namespace Evently.Modules.Users.Application.Users.ChangeUserName;

public class UserNameChangedDomainEventHandler (
    IEventBus eventBus)
    : IDomainEventHandler<UserNameChangedDomainEvent>
{
    public async Task Handle(UserNameChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(
            new UserProfileUpdatedIntegrationEvent(
                notification.Id,
                notification.OccuredAtUtc,
                notification.UserId,
                notification.FirstName,
                notification.LastName),
            cancellationToken);
    }
}