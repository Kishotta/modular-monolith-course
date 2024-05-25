using Evently.Common.Application.Exceptions;
using Evently.Common.Infrastructure.Auditing;
using Microsoft.AspNetCore.Http;

namespace Evently.Common.Infrastructure.Authentication;

public class JwtAuditingUserProvider(IHttpContextAccessor httpContextAccessor) : IAuditingUserProvider
{
    private const string DefaultUser = "Unknown User";
    
    public string GetUserId()
    {
        try
        {
            return httpContextAccessor.HttpContext?.User.GetUserId().ToString() ?? DefaultUser;
        }
        catch (EventlyException)
        {
            return DefaultUser;
        }
    }
}