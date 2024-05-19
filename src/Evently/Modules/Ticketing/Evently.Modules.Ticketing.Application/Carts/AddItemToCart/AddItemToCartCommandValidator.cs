using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

internal sealed class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(command => command.CustomerId).NotEmpty();
        RuleFor(command => command.TicketTypeId).NotEmpty();
        RuleFor(command => command.Quantity).GreaterThan(decimal.Zero);
    }
}