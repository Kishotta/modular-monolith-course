using Evently.Modules.Events.Application.Events.SearchEvents;

namespace Evently.Modules.Events.Presentation.Events;

public class SearchEvents : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/search", async (
            ISender sender,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate,
            int page = 0,
            int pageSize = 15) =>
        {
            var result = await sender.Send(
                new SearchEventsQuery(categoryId, startDate, endDate, page, pageSize));

            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        .WithName(nameof(SearchEvents))
        .WithTags(Tags.Events);
    }
}