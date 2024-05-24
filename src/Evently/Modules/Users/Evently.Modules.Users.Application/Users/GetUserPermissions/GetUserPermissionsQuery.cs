using Evently.Common.Application.Authorization;

namespace Evently.Modules.Users.Application.Users.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionResponse>;