using Evently.Modules.Events.Application.TicketTypes.GetTicketTypes;

namespace Evently.Modules.Events.Presentation.TicketTypes;

public class GetTicketTypes : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ticket-types", async (Guid eventId, ISender sender) =>
            {
                var result = await sender.Send(new GetTicketTypesQuery(eventId));

                return result.Match(Results.Ok, Common.Presentation.ApiResults.ApiResults.Problem);
            })
            .WithName(nameof(GetTicketTypes))
            .WithTags(Tags.TicketTypes);
    }
}