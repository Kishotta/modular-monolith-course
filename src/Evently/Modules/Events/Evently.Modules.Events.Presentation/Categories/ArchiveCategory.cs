using Evently.Modules.Events.Application.Categories.ArchiveCategory;

namespace Evently.Modules.Events.Presentation.Categories;

internal class ArchiveCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("categories/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ArchiveCategoryCommand(id));
            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization()
        .WithName(nameof(ArchiveCategory))
        .WithTags(Tags.Categories)
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            
            Summary = "Archive Category",
            Description = "Archive a Category so that it can no longer be used for new Events"
        });
    }
}