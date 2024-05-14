using FluentValidation;

namespace Evently.Modules.Events.Application.TicketTypes.CreateTicketType;

internal sealed class CreateTicketTypeCommandValidator : AbstractValidator<CreateTicketTypeCommand>
{
    public CreateTicketTypeCommandValidator()
    {
        RuleFor(command => command.EventId).NotEmpty();
        RuleFor(command => command.Name).NotEmpty();
        RuleFor(command => command.Price).GreaterThan(decimal.Zero);
        RuleFor(command => command.Currency).NotEmpty();
        RuleFor(command => command.Quantity).GreaterThan(decimal.Zero);
    }
}