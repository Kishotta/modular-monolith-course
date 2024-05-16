using Evently.Modules.Events.Application.Events.CancelEvent;

namespace Evently.Modules.Events.Presentation.Events;

internal class CancelEvent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("events/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new CancelEventCommand(id));

                return result.Match(Results.NoContent, ApiResults.Problem);
            })
            .WithName(nameof(CancelEvent))
            .WithTags(Tags.Events);
    }
}