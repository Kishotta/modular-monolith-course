using Evently.Common.Application.Messaging;

namespace Evently.Modules.Users.Application.Users.UpdateUser;

public record UpdateUserCommand(Guid UserId, string FirstName, string LastName) : ICommand<UserResponse>;