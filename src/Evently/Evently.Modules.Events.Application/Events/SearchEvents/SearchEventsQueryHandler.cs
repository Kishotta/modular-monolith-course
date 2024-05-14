using System.Data;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Domain;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.SearchEvents;

internal sealed class SearchEventsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<SearchEventsQuery, SearchEventsResponse>
{
    public async Task<Result<SearchEventsResponse>> Handle(SearchEventsQuery request, CancellationToken cancellationToken)
    {
        await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        var parameters = new SearchEventsParameters(
            (int)EventStatus.Published,
            request.CategoryId,
            request.StartDate?.Date,
            request.EndDate?.Date,
            request.PageSize,
            (request.Page - 1) * request.PageSize);

        var events = await GetEventsAsync(dbConnection, parameters);

        var totalCount = await CountEventsAsync(dbConnection, parameters);

        return new SearchEventsResponse(request.Page, request.PageSize, totalCount, events);
    }
    
    private async Task<IReadOnlyCollection<GetEvents.EventResponse>> GetEventsAsync(
        IDbConnection dbConnection, 
        SearchEventsParameters parameters)
    {
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
             WHERE
                 status = @Status AND
                 (@CategoryId IS NULL OR category_id = @CategoryId) AND
                 (@StartDate::timestamp IS NULL OR starts_at_utc >= @StartDate::timestamp) AND
                 (@EndDate::timestamp IS NULL OR ends_at_utc >= @EndDate::timestamp)
             ORDER BY starts_at_utc
             OFFSET @Skip
             LIMIT @Take
             """;

        var events = (await dbConnection.QueryAsync<GetEvents.EventResponse>(sql, parameters)).AsList();

        return events;
    }
    
    private async Task<int> CountEventsAsync(
        IDbConnection dbConnection, 
        SearchEventsParameters parameters)
    {
        const string sql =
            $"""
            SELECT COUNT(*)
            FROM events.events
            WHERE
                status = @Status AND
                (@CategoryId IS NULL OR category_id = @CategoryId) AND
                (@StartDate::timestamp IS NULL OR starts_at_utc >= @StartDate::timestamp) AND
                (@EndDate::timestamp IS NULL OR ends_at_utc >= @EndDate::timestamp)
            """;

        var totalCount = await dbConnection.ExecuteScalarAsync<int>(sql, parameters);

        return totalCount;
    }

    private sealed record SearchEventsParameters(
        int Status,
        Guid? CategoryId,
        DateTime? StartDate,
        DateTime? EndDate,
        int Take,
        int Skip);
}

