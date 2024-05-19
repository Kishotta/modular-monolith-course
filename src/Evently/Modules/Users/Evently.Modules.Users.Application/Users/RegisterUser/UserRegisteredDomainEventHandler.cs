using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Modules.Ticketing.PublicApi;
using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.Domain.Users;
using MediatR;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

public class UserRegisteredDomainEventHandler(
    ISender sender,
    ITicketingApi ticketingApi) 
    : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetUserQuery(notification.UserId), cancellationToken);
        if (result.IsFailure) 
            throw new EventlyException(nameof(GetUserQuery), result.Error);
        
        await ticketingApi.CreateCustomerAsync(
            result.Value.Id, 
            result.Value.Email,
            result.Value.FirstName,
            result.Value.LastName, 
            cancellationToken);

    }
}