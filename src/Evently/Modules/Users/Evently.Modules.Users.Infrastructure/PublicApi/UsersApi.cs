using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.PublicApi;
using MediatR;

namespace Evently.Modules.Users.Infrastructure.PublicApi;

internal sealed class UsersApi(ISender sender) : IUsersApi
{
    public async Task<UserResponse?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetUserQuery(userId), cancellationToken);

        if (result.IsFailure) return null;

        return new UserResponse(
            result.Value.Id, 
            result.Value.Email, 
            result.Value.FirstName, 
            result.Value.LastName);
    }
}