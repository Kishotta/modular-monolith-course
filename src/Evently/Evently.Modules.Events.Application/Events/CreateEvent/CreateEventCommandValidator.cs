using FluentValidation;

namespace Evently.Modules.Events.Application.Events.CreateEvent;

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(command => command.Title).NotEmpty();
        RuleFor(command => command.Description).NotEmpty();
        RuleFor(command => command.Location).NotEmpty();
        RuleFor(command => command.StartsAtUtc).NotEmpty();
        RuleFor(command => command.EndsAtUtc)
            .Must((command, endsAtUtc) => endsAtUtc > command.StartsAtUtc)
            .When(command => command.EndsAtUtc.HasValue);
    }
}