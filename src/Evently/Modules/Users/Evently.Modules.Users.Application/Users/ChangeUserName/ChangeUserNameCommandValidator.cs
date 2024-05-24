using FluentValidation;

namespace Evently.Modules.Users.Application.Users.ChangeUserName;

internal sealed class ChangeUserNameCommandValidator : AbstractValidator<ChangeUserNameCommand>
{
    public ChangeUserNameCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.FirstName).NotEmpty();
        RuleFor(command => command.LastName).NotEmpty();
    }
}