using FluentValidation;

namespace Evently.Modules.Events.Application.Events.PublishEvent;

public class PublishEventCommandValidator : AbstractValidator<PublishEventCommand>
{
    public PublishEventCommandValidator()
    {
        RuleFor(command => command.EventId).NotEmpty();
    }
}