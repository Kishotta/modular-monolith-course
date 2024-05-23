using Evently.Modules.Events.Application.Categories.GetCategories;

namespace Evently.Modules.Events.Presentation.Categories;

internal class GetCategories : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ISender sender) =>
            {
                var result = await sender.Send(new GetCategoriesQuery());

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization()
            .WithName(nameof(GetCategories))
            .WithTags(Tags.Categories)
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Get Categories",
                Description = "Get all categories",
            });
    }
}