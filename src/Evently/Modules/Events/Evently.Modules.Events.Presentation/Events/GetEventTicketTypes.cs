using Evently.Modules.Events.Application.TicketTypes.GetTicketTypes;

namespace Evently.Modules.Events.Presentation.Events;

internal class GetEventTicketTypes : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}/ticket-types", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetTicketTypesQuery(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .WithName(nameof(GetEventTicketTypes))
            .WithTags(Tags.Events);
    }
}