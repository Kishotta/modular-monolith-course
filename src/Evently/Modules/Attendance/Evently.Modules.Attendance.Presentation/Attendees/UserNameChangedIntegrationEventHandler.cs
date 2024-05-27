using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Attendance.Application.Attendees.ChangeAttendeeName;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Attendees;

public sealed class UserNameChangedIntegrationEventHandler(ISender sender)
    : IIntegrationEventHandler<UserNameChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserNameChangedIntegrationEvent> context)
    {
        var result = await sender.Send(
            new ChangeAttendeeNameCommand(
                context.Message.UserId,
                context.Message.FirstName,
                context.Message.LastName),
            context.CancellationToken);

        if (result.IsFailure)
            throw new EventlyException(nameof(ChangeAttendeeNameCommand), result.Error);
    }
}