using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Customers.CreateCustomer;

internal sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(command => command.CustomerId).NotEmpty();
        RuleFor(command => command.Email).NotEmpty().EmailAddress();
        RuleFor(command => command.FirstName).NotEmpty();
        RuleFor(command => command.LastName).NotEmpty();
    }
}