using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Tickets.GetTicketsForOrder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Tickets;

internal sealed class GetTicketsForOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("tickets/order/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetTicketsForOrderQuery(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Tickets);
    }
}
