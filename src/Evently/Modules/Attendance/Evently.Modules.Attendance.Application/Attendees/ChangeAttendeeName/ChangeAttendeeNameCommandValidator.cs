using FluentValidation;

namespace Evently.Modules.Attendance.Application.Attendees.ChangeAttendeeName;

internal sealed class ChangeAttendeeNameCommandValidator : AbstractValidator<ChangeAttendeeNameCommand>
{
    public ChangeAttendeeNameCommandValidator()
    {
        RuleFor(command => command.AttendeeId).NotEmpty();
        RuleFor(command => command.FirstName).NotEmpty();
        RuleFor(command => command.LastName).NotEmpty();
    }
}