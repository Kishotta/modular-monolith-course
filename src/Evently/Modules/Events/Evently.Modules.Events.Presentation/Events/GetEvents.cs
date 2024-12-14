using Evently.Modules.Events.Application.Events.GetEvents;

namespace Evently.Modules.Events.Presentation.Events;

internal class GetEvents : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events", async (int page, int perPage, ISender sender) =>
        {
            var result = await sender.Send(new GetEventsQuery(page, perPage));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization()
        .WithName(nameof(GetEvents))
        .WithTags(Tags.Events);
    }
}