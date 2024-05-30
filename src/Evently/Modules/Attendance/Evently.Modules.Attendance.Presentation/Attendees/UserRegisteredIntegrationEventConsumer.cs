using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Attendance.Application.Attendees.CreateAttendee;
using Evently.Modules.Users.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Attendees;

public sealed class UserRegisteredIntegrationEventConsumer(ISender sender)
    : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async Task Handle(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new CreateAttendeeCommand(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.FirstName,
                integrationEvent.LastName),
            cancellationToken);

        if (result.IsFailure)
            throw new EventlyException(nameof(CreateAttendeeCommand), result.Error);
    }
}