using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Ticketing.Application.Customers.ChangeCustomerName;
using Evently.Modules.Users.IntegrationEvents;

namespace Evently.Modules.Ticketing.Presentation.Customers;

public sealed class UserProfileUpdatedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<UserNameChangedIntegrationEvent>
{
    public override async Task Handle(UserNameChangedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new ChangeCustomerNameCommand(
                integrationEvent.UserId,
                integrationEvent.FirstName,
                integrationEvent.LastName), 
            cancellationToken);

        if (result.IsFailure)
            throw new EventlyException(nameof(ChangeCustomerNameCommand), result.Error);
    }
}