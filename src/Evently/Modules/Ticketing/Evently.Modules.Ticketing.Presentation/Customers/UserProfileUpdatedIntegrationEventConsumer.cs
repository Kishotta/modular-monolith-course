using Evently.Common.Application.Exceptions;
using Evently.Modules.Ticketing.Application.Customers.ChangeCustomerName;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;

namespace Evently.Modules.Ticketing.Presentation.Customers;

public class UserProfileUpdatedIntegrationEventConsumer(ISender sender)
    : IConsumer<UserNameChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserNameChangedIntegrationEvent> context)
    {
        var result = await sender.Send(
            new ChangeCustomerNameCommand(
                context.Message.UserId,
                context.Message.FirstName,
                context.Message.LastName), 
            context.CancellationToken);

        if (result.IsFailure)
            throw new EventlyException(nameof(ChangeCustomerNameCommand), result.Error);
    }
}