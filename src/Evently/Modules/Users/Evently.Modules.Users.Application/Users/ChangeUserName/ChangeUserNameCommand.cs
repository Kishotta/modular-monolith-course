namespace Evently.Modules.Users.Application.Users.ChangeUserName;

public record ChangeUserNameCommand(Guid UserId, string FirstName, string LastName) : ICommand<UserResponse>;