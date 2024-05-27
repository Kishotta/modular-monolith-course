using Evently.Common.Application.Exceptions;
using Evently.Modules.Attendance.Application.Attendees.CreateAttendee;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Attendees;

public sealed class UserRegisteredIntegrationEventConsumer(ISender sender)
    : IConsumer<UserRegisteredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        var result = await sender.Send(
            new CreateAttendeeCommand(
                context.Message.UserId,
                context.Message.Email,
                context.Message.FirstName,
                context.Message.LastName),
            context.CancellationToken);

        if (result.IsFailure)
            throw new EventlyException(nameof(CreateAttendeeCommand), result.Error);
    }
}