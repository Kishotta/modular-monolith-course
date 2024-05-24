using Evently.Modules.Users.Application.Users.RegisterUser;

namespace Evently.Modules.Users.Presentation.Users;

internal sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (
                Request request, 
                ISender sender, 
                HttpContext context, 
                LinkGenerator linkGenerator) =>
            {
                var result = await sender.Send(new RegisterUserCommand(
                    request.Email,
                    request.Password,
                    request.FirstName,
                    request.LastName));

                return result.Match(
                    () => Results.Created(
                        linkGenerator.GetUriByName(context, nameof(GetUserProfile), new { id = result.Value.Id }),
                        result.Value),
                    ApiResults.Problem);
            })
            .AllowAnonymous()
            .WithName(nameof(RegisterUser))
            .WithTags(Tags.Users);
    }

    internal sealed record Request(string Email, string Password, string FirstName, string LastName);
}