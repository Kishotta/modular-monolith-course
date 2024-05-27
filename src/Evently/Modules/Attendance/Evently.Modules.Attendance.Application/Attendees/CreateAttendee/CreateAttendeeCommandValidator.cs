using FluentValidation;

namespace Evently.Modules.Attendance.Application.Attendees.CreateAttendee;

internal sealed class CreateAttendeeCommandValidator : AbstractValidator<CreateAttendeeCommand>
{
    public CreateAttendeeCommandValidator()
    {
        RuleFor(command => command.AttendeeId).NotEmpty();
        RuleFor(command => command.Email).EmailAddress();
        RuleFor(command => command.FirstName).NotEmpty();
        RuleFor(command => command.LastName).NotEmpty();
    }
}