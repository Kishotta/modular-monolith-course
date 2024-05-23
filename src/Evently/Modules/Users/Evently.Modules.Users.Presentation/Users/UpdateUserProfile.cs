using Evently.Modules.Users.Application.Users.UpdateUser;

namespace Evently.Modules.Users.Presentation.Users;

internal sealed class UpdateUserProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/{id:guid}/profile", async (Guid id, Request request, ISender sender) =>
            {
                var result = await sender.Send(new UpdateUserCommand(
                    id,
                    request.FirstName,
                    request.LastName));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization()
            .WithName(nameof(UpdateUserCommand))
            .WithTags(Tags.Users);
    }

    internal sealed record Request(string FirstName, string LastName);
}