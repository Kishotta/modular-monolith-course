using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users;

public sealed record UserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName)
{
    public static implicit operator UserResponse(User user) =>
        new(
            user.Id,
            user.Email, 
            user.FirstName, 
            user.LastName);
}