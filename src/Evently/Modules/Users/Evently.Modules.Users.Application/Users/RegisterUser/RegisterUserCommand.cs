namespace Evently.Modules.Users.Application.Users.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : ICommand<UserResponse>;