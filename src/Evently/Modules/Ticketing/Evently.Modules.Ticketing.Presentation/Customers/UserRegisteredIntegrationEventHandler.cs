using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Ticketing.Application.Customers.CreateCustomer;
using Evently.Modules.Users.IntegrationEvents;

namespace Evently.Modules.Ticketing.Presentation.Customers;

public sealed class UserRegisteredIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async Task Handle(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new CreateCustomerCommand(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.FirstName,
                integrationEvent.LastName), 
            cancellationToken);

        if (result.IsFailure)
            throw new EventlyException(nameof(CreateCustomerCommand), result.Error);
    }
}