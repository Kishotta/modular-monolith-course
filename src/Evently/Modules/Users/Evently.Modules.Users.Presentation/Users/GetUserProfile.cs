using Evently.Modules.Users.Application.Users.GetUser;

namespace Evently.Modules.Users.Presentation.Users;

internal sealed class GetUserProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{id:guid}/profile", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetUserQuery(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization()
            .WithName(nameof(GetUserProfile))
            .WithTags(Tags.Users);
    }
}