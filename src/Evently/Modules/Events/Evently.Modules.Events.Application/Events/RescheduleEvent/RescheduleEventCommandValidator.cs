using FluentValidation;

namespace Evently.Modules.Events.Application.Events.RescheduleEvent;

public class RescheduleEventCommandValidator : AbstractValidator<RescheduleEventCommand>
{
    public RescheduleEventCommandValidator()
    {
        RuleFor(command => command.EventId).NotEmpty();
        RuleFor(command => command.StartsAtUtc).NotEmpty();
        RuleFor(command => command.EndsAtUtc)
            .GreaterThanOrEqualTo(command => command.StartsAtUtc)
            .When(command => command.EndsAtUtc.HasValue);
    }
}