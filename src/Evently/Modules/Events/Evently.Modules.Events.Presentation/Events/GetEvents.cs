using Evently.Modules.Events.Application.Events.GetEvents;

namespace Evently.Modules.Events.Presentation.Events;

internal class GetEvents : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events", async (ISender sender) =>
        {
            var result = await sender.Send(new GetEventsQuery());

            return result.Match(Results.Ok, Common.Presentation.ApiResults.ApiResults.Problem);
        })
        .WithName(nameof(GetEvents))
        .WithTags(Tags.Events);
    }
}