using Evently.Modules.Events.Application.Categories.GetCategory;

namespace Evently.Modules.Events.Presentation.Categories;

internal class GetCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new GetCategoryQuery(id);

                var result = await sender.Send(query);

                return result.Match(
                    Results.Ok,
                    Common.Presentation.ApiResults.ApiResults.Problem);
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