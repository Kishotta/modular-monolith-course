using Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal class UpdateTicketTypePrice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("ticket-types/{id:guid}/price", async (Guid id, Request request, ISender sender) =>
            {
                var result = await sender.Send(new UpdateTicketTypePriceCommand(id, request.Price));

                return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
            })
            .WithName(nameof(UpdateTicketTypePrice))
            .WithTags(Tags.TicketTypes);
    }

    internal sealed record Request(decimal Price);
}