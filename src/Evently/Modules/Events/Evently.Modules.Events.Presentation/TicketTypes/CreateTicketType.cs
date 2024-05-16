using Evently.Modules.Events.Application.TicketTypes.CreateTicketType;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal class CreateTicketType : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("ticket-types", async (Request request, ISender sender) =>
            {
                var result = await sender.Send(new CreateTicketTypeCommand(
                    request.EventId,
                    request.Name,
                    request.Price,
                    request.Currency,
                    request.Quantity));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .WithName(nameof(CreateTicketType))
            .WithTags(Tags.TicketTypes);
    }

    internal sealed record Request(Guid EventId, string Name, decimal Price, string Currency, decimal Quantity);
}