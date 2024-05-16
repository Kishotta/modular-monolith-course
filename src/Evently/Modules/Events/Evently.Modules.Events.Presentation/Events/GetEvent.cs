using Evently.Modules.Events.Application.Events.GetEvent;

namespace Evently.Modules.Events.Presentation.Events;

internal class GetEvent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetEventQuery(id));
                
                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .WithName(nameof(GetEvent))
            .WithTags(Tags.Events);
    }
}