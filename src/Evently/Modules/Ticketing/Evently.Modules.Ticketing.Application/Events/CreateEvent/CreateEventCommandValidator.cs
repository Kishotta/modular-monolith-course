using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Events.CreateEvent;

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(command => command.EventId).NotEmpty();
        RuleFor(command => command.Title).NotEmpty();
        RuleFor(command => command.Description).NotEmpty();
        RuleFor(command => command.Location).NotEmpty();
        RuleFor(command => command.StartsAtUtc).NotEmpty();
        RuleFor(command => command.EndsAtUtc).Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc).When(c => c.EndsAtUtc.HasValue);

        RuleForEach(command => command.TicketTypes)
            .ChildRules(t =>
            {
                t.RuleFor(ticketType => ticketType.EventId).NotEmpty();
                t.RuleFor(ticketType => ticketType.Name).NotEmpty();
                t.RuleFor(ticketType => ticketType.Price).GreaterThan(decimal.Zero);
                t.RuleFor(ticketType => ticketType.Currency).NotEmpty();
                t.RuleFor(ticketType => ticketType.Quantity).GreaterThan(decimal.Zero);
            });
    }
}