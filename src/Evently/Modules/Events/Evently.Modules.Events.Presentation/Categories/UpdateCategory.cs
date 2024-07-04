using Evently.Modules.Events.Application.Categories.UpdateCategory;

namespace Evently.Modules.Events.Presentation.Categories;

internal class UpdateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id:guid}", async (Guid id, string name, ISender sender) =>
            {
                var result = await sender.Send(new UpdateCategoryCommand(id, name));

                return result.Match(
                    Results.Ok,
                    ApiResults.Problem);
            })
            .RequireAuthorization()
            .WithName(nameof(UpdateCategory))
            .WithTags(Tags.Categories)
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Update Category",
                Description = "Update the name of a category.",
            });
    }
}