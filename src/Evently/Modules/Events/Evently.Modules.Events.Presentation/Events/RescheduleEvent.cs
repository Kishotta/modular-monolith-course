using Evently.Modules.Events.Application.Events.RescheduleEvent;

namespace Evently.Modules.Events.Presentation.Events;

internal class RescheduleEvent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("events/{id:guid}/reschedule", async (Guid id, Request request, ISender sender) =>
        {
            var result = await sender.Send(new RescheduleEventCommand(id, request.StartsAtUtc, request.EndsAtUtc));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithName(nameof(RescheduleEvent))
        .WithTags(Tags.Events);
    }
    internal sealed record Request(DateTime StartsAtUtc, DateTime? EndsAtUtc);
}