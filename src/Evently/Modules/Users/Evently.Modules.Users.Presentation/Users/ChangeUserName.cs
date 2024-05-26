using Evently.Modules.Users.Application.Users.ChangeUserName;

namespace Evently.Modules.Users.Presentation.Users;

internal sealed class ChangeUserName : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/{id:guid}/profile", async (Guid id, Request request, ISender sender) =>
            {
                var result = await sender.Send(new ChangeUserNameCommand(
                    id,
                    request.FirstName,
                    request.LastName));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization(Permissions.ChangeUserName)
            .WithName(nameof(ChangeUserNameCommand))
            .WithTags(Tags.Users);
    }

    internal sealed record Request(string FirstName, string LastName);
}