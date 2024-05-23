using Evently.Modules.Events.Application.TicketTypes.GetTicketType;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal class GetTicketType : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ticket-types/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetTicketTypeQuery(id));

                return result.Match(Results.Ok, Common.Presentation.ApiResults.ApiResults.Problem);
            })
            .RequireAuthorization()
            .WithName(nameof(GetTicketType))
            .WithTags(Tags.TicketTypes);
    }
}