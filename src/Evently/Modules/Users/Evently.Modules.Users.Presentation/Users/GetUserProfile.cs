using System.Security.Claims;
using Evently.Common.Infrastructure.Authentication;
using Evently.Modules.Users.Application.Users.GetUser;

namespace Evently.Modules.Users.Presentation.Users;

internal sealed class GetUserProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/profile", async (ClaimsPrincipal claims, ISender sender) =>
            {
                var result = await sender.Send(new GetUserQuery(claims.GetUserId()));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization("users:read")
            .WithName(nameof(GetUserProfile))
            .WithTags(Tags.Users);
    }
}