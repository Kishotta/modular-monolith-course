using Evently.Modules.Events.Application.TicketTypes.CreateTicketType;

namespace Evently.Modules.Events.Presentation.Events;

internal class CreateEventTicketType : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events/{id:guid}/ticket-types", async (Guid id, Request request, ISender sender) =>
            {
                var result = await sender.Send(new CreateTicketTypeCommand(
                    id,
                    request.Name,
                    request.Price,
                    request.Currency,
                    request.Quantity));

                return result.Match(Results.Ok, Common.Presentation.ApiResults.ApiResults.Problem);
            })
            .WithName(nameof(CreateEventTicketType))
            .WithTags(Tags.Events);
    }

    internal sealed record Request(string Name, decimal Price, string Currency, decimal Quantity);
}