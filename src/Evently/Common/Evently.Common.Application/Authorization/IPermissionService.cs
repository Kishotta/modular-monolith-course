using Evently.Common.Domain;

namespace Evently.Common.Application.Authorization;

public interface IPermissionService
{
    Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId);
}