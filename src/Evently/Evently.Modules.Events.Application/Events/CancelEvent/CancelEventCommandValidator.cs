using FluentValidation;

namespace Evently.Modules.Events.Application.Events.CancelEvent;

internal sealed class CancelEventCommandValidator : AbstractValidator<CancelEventCommand>
{
    public CancelEventCommandValidator()
    {
        RuleFor(command => command.EventId).NotEmpty();
    }
}