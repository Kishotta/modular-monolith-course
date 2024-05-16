using Evently.Modules.Events.Application.Events.CancelEvent;

namespace Evently.Modules.Events.Presentation.Events;

internal class CancelEvent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("events/{id:guid}", async (Guid id, ISender sender) =>
            {
                var command = new CancelEventCommand(id);

                var result = await sender.Send(command);

                return result.Match(
                    Results.NoContent, 
                    Common.Presentation.ApiResults.ApiResults.Problem);
            })
            .WithName(nameof(CancelEvent))
            .WithTags(Tags.Events);
    }
}