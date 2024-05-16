using Evently.Common.Application.Messaging;

namespace Evently.Modules.Users.Application.Users.GetUser;

public record GetUserQuery(Guid UserId) : IQuery<UserResponse>;