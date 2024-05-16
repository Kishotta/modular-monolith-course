using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.GetUser;

internal sealed class GetUserQueryHandler(
    IDbConnectionFactory dbConnectionFactory
    ) : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();
        
        const string sql =
            $"""
             SELECT
                id as {nameof(UserResponse.Id)},
                email as {nameof(UserResponse.Email)},
                first_name as {nameof(UserResponse.FirstName)},
                last_name as {nameof(UserResponse.LastName)}
             FROM users.users
             WHERE id = @UserId
             """;
        
        var user = await dbConnection.QuerySingleOrDefaultAsync<UserResponse>(sql, request);

        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFound(request.UserId));

        return user;
    }
}