using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

internal sealed class UserRegisteredDomainEventHandler(
    ISender sender,
    IEventBus eventBus) 
    : DomainEventHandler<UserRegisteredDomainEvent>
{
    public override async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetUserQuery(notification.UserId), cancellationToken);
        if (result.IsFailure) 
            throw new EventlyException(nameof(GetUserQuery), result.Error);

        var user = result.Value;

        await eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                notification.Id,
                notification.OccuredAtUtc,
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName), 
            cancellationToken);
    }
}