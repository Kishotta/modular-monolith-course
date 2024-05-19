using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Carts.AddItemToCart;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Carts;

internal sealed class AddToCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("carts/add", async (Request request, ISender sender) =>
            {
                var result = await sender.Send(new AddItemToCartCommand(
                    request.CustomerId,
                    request.TicketTypeId,
                    request.Quantity));

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            })
            .WithName(nameof(AddToCart))
            .WithTags(Tags.Carts);
    }

    internal sealed record Request(Guid CustomerId, Guid TicketTypeId, decimal Quantity);
}