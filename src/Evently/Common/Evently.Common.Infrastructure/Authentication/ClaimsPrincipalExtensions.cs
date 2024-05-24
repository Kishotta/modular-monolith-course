using System.Security.Claims;
using Evently.Common.Application.Exceptions;

namespace Evently.Common.Infrastructure.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(CustomClaims.Sub);

        return Guid.TryParse(userId, out var parsedUserId)
            ? parsedUserId
            : throw new EventlyException("User identifier is unavailable");
    }
    
    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
               throw new EventlyException("User identity is unavailable");
    }

    public static HashSet<string> GetPermissions(this ClaimsPrincipal? principal)
    {
        var permissionClaims = principal?.FindAll(CustomClaims.Permission) ??
                               throw new EventlyException("Permissions are unavailable");
        return permissionClaims.Select(claim => claim.Value).ToHashSet();
    }
}