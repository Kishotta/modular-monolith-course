using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Customers.ChangeCustomerName;

internal sealed class ChangeCustomerNameCommandValidator : AbstractValidator<ChangeCustomerNameCommand>
{
    public ChangeCustomerNameCommandValidator()
    {
        RuleFor(command => command.CustomerId).NotEmpty();
        RuleFor(command => command.FirstName).NotEmpty();
        RuleFor(command => command.LastName).NotEmpty();
    }
}