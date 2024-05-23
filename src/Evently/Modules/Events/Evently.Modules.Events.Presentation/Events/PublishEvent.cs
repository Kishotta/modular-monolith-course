using Evently.Modules.Events.Application.Events.PublishEvent;

namespace Evently.Modules.Events.Presentation.Events;

internal class PublishEvent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("events/{id:guid}/publish", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new PublishEventCommand(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization()
            .WithName(nameof(PublishEvent))
            .WithTags(Tags.Events);
    }
}