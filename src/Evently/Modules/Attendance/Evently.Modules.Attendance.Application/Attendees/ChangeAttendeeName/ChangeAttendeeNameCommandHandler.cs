using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Attendees;

namespace Evently.Modules.Attendance.Application.Attendees.ChangeAttendeeName;

internal sealed class ChangeAttendeeNameCommandHandler(
    IAttendeeRepository attendeeRepository, 
    IUnitOfWork unitOfWork)
    : ICommandHandler<ChangeAttendeeNameCommand>
{
    public async Task<Result> Handle(ChangeAttendeeNameCommand request, CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetAsync(request.AttendeeId, cancellationToken);
        if (attendee is null)
            return Result.Failure(AttendeeErrors.NotFound(request.AttendeeId));

        attendee.ChangeName(request.FirstName, request.LastName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}