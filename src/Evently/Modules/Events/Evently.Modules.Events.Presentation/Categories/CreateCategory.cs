using Evently.Modules.Events.Application.Categories.CreateCategory;

namespace Evently.Modules.Events.Presentation.Categories;

internal class CreateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async (Request request, ISender sender, HttpContext context, LinkGenerator linkGenerator) =>
            {
                var command = new CreateCategoryCommand(request.Name);

                var result = await sender.Send(command);

                return result.Match(
                    () => Results.Created(
                        linkGenerator.GetUriByName(context, nameof(GetCategory), new { id = result.Value.Id }), 
                        result.Value),
                    Common.Presentation.ApiResults.ApiResults.Problem);
            })
            .WithName(nameof(CreateCategory))
            .WithTags(Tags.Categories)
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Create Category",
                Description = "Create a new Category so that it can be used for new Events"
            });
    }

    internal sealed record Request(string Name);
}