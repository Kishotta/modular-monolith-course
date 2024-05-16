using Evently.Common.Application.Messaging;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

public record RegisterUserCommand(
    string Email, 
    string FirstName,
    string LastName) : ICommand<UserResponse>;