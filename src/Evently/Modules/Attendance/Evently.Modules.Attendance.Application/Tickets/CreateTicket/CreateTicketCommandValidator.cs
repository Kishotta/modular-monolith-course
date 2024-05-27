using FluentValidation;

namespace Evently.Modules.Attendance.Application.Tickets.CreateTicket;

internal sealed class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(command => command.TicketId).NotEmpty();
        RuleFor(command => command.AttendeeId).NotEmpty();
        RuleFor(command => command.EventId).NotEmpty();
        RuleFor(command => command.Code).NotEmpty();
    }
}