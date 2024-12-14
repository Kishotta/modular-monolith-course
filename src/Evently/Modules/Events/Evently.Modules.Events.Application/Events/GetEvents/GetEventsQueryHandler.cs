using Dapper;
using Evently.Common.Application.Data;

namespace Evently.Modules.Events.Application.Events.GetEvents;

internal sealed class GetEventsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IPaginatedQueryHandler<GetEventsQuery, IReadOnlyCollection<EventResponse>>
{
    public async Task<Result<PaginatedResponse<IReadOnlyCollection<EventResponse>>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                id AS {nameof(EventResponse.Id)},
                category_id AS {nameof(EventResponse.CategoryId)},
                title AS {nameof(EventResponse.Title)},
                description AS {nameof(EventResponse.Description)},
                location AS {nameof(EventResponse.Location)},
                starts_at_utc AS {nameof(EventResponse.StartsAtUtc)},
                ends_at_utc AS {nameof(EventResponse.EndsAtUtc)}
             FROM events.events
             LIMIT @Limit
             OFFSET @Offset
             """;

        var events = (await dbConnection.QueryAsync<EventResponse>(sql, request.PaginationFilter)).AsList();
        
        const string countSql = "SELECT COUNT(*) FROM events.events";
        var totalCount = await dbConnection.ExecuteScalarAsync<int>(countSql);
        

        return PaginatedResponse<IReadOnlyCollection<EventResponse>>.Create(
            events,
            request,
            totalCount);
    }
}