using Evently.Modules.Events.Application.Categories.GetCategory;

namespace Evently.Modules.Events.Presentation.Categories;

internal class GetCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetCategoryQuery(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .WithName(nameof(GetCategory))
            .WithTags(Tags.Categories)
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Get Category",
                Description = "Get a category by its id.",
            });
    }
}