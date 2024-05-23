using Evently.Common.Application.Exceptions;
using Evently.Modules.Ticketing.Application.Customers.UpdateCustomer;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;

namespace Evently.Modules.Ticketing.Presentation.Customers;

public class UserProfileUpdatedIntegrationEventConsumer(ISender sender)
    : IConsumer<UserProfileUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserProfileUpdatedIntegrationEvent> context)
    {
        var result = await sender.Send(
            new UpdateCustomerCommand(
                context.Message.UserId,
                context.Message.FirstName,
                context.Message.LastName), 
            context.CancellationToken);

        if (result.IsFailure)
            throw new EventlyException(nameof(UpdateCustomerCommand), result.Error);
    }
}