using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.IntegrationEvents;

namespace Evently.Modules.Users.Application.Users.UpdateUser;

public class UserProfileUpdatedDomainEventHandler (
    IEventBus eventBus)
    : IDomainEventHandler<UserProfileUpdatedDomainEvent>
{
    public async Task Handle(UserProfileUpdatedDomainEvent notification, CancellationToken cancellationToken)
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