using System.Security.Claims;
using Evently.Common.Infrastructure.Auditing;

namespace Evently.Api;

public class UserProvider(IHttpContextAccessor httpContextAccessor) : IAuditUserProvider
{
    public string GetUserId()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown User";
    }
}