using System.Security.Claims;
using Evently.Common.Application.Exceptions;
using Evently.Common.Infrastructure.Auditing;
using Microsoft.AspNetCore.Http;

namespace Evently.Common.Infrastructure.Authentication;

public class JwtAuditingUserProvider(IHttpContextAccessor httpContextAccessor) : IAuditingUserProvider
{
    public string GetUserId()
    {
        try
        {
            return httpContextAccessor.HttpContext?.User.GetUserId().ToString()!;
        }
        catch (EventlyException)
        {
            return "Unknown User";
        }
    }
}