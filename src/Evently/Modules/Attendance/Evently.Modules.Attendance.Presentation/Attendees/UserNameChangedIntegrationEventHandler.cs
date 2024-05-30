using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Modules.Attendance.Application.Attendees.ChangeAttendeeName;
using Evently.Modules.Users.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Attendees;

public sealed class UserNameChangedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<UserNameChangedIntegrationEvent>
{
    public override async Task Handle(UserNameChangedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new ChangeAttendeeNameCommand(
                integrationEvent.UserId,
                integrationEvent.FirstName,
                integrationEvent.LastName),
            cancellationToken);

        if (result.IsFailure)
            throw new EventlyException(nameof(ChangeAttendeeNameCommand), result.Error);
    }
}