using Evently.Modules.Events.Application.Events.CreateEvent;

namespace Evently.Modules.Events.Presentation.Events;

internal class CreateEvent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (Request request, ISender sender) =>
            {
                var command = new CreateEventCommand(
                    request.CategoryId,
                    request.Title,
                    request.Description,
                    request.Location,
                    request.StartsAtUtc,
                    request.EndsAtUtc);

                var result = await sender.Send(command);

                return result.Match(() => Results.Created(result.Value.Id.ToString(), result.Value), ApiResults.Problem);
            })
            .WithName(nameof(CreateEvent))
            .WithTags(Tags.Events);
    }

    internal sealed record Request(
        Guid CategoryId,
        string Title,
        string Description,
        string Location,
        DateTime StartsAtUtc,
        DateTime? EndsAtUtc);
}