using System.Security.Claims;
using Evently.Common.Infrastructure.Auditing;
using Microsoft.AspNetCore.Http;

namespace Evently.Common.Infrastructure.Authentication;

public class JwtAuditingUserProvider(IHttpContextAccessor httpContextAccessor) : IAuditingUserProvider
{
    public string GetUserId()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown User";
    }
}