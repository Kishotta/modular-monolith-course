using Dapper;
using Evently.Common.Application.Authorization;
using Evently.Common.Application.Data;
using Evently.Common.Domain;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.GetUserPermissions;

internal sealed class GetUserPermissionsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserPermissionsQuery, PermissionResponse>
{
    public async Task<Result<PermissionResponse>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT DISTINCT
                u.id AS {nameof(UserPermission.UserId)},
                rp.permission_code AS {nameof(UserPermission.Permission)}
             FROM users.users u
             JOIN users.user_roles ur ON u.id = ur.user_id
             JOIN users.role_permissions rp ON ur.role_name = rp.role_name
             WHERE u.identity_id = @IdentityId
             """;

        var permissions = (await connection.QueryAsync<UserPermission>(sql, request)).ToList();
        if (permissions.Count == 0)
            return UserErrors.NotFound(request.IdentityId);

        return new PermissionResponse(permissions[0].UserId,
            permissions.Select(userPermission => userPermission.Permission).ToHashSet());
    }

    internal sealed class UserPermission
    {
        internal Guid UserId { get; init; }
        internal string Permission { get; init; } = string.Empty;
    }
}