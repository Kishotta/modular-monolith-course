using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Domain;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.GetEvent;

internal sealed class GetEventQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetEventQuery, EventResponse>
{
    public async Task<Result<EventResponse>> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();
        
        const string sql =
            $"""
             SELECT
                e.id AS {nameof(EventResponse.Id)},
                e.category_id AS {nameof(EventResponse.CategoryId)},
                e.title AS {nameof(EventResponse.Title)},
                e.description AS {nameof(EventResponse.Description)},
                e.location AS {nameof(EventResponse.Location)},
                e.starts_at_utc AS {nameof(EventResponse.StartsAtUtc)},
                e.ends_at_utc AS {nameof(EventResponse.EndsAtUtc)},
                e.status AS {nameof(EventResponse.Status)},
                tt.id AS {nameof(TicketTypeResponse.TicketTypeId)},
                tt.name AS {nameof(TicketTypeResponse.Name)},
                tt.price AS {nameof(TicketTypeResponse.Price)},
                tt.currency AS {nameof(TicketTypeResponse.Currency)},
                tt.quantity AS {nameof(TicketTypeResponse.Quantity)}
             FROM events.events e
             LEFT JOIN events.ticket_types tt ON e.id = tt.event_id
             WHERE e.id = @EventId
             """;

        Dictionary<Guid, EventResponse> eventsDictionary = [];
        await connection.QueryAsync<EventResponse, TicketTypeResponse?, EventResponse>(
            sql,
            (@event, ticketType) =>
            {
                if (eventsDictionary.TryGetValue(@event.Id, out var existingEvent))
                    @event = existingEvent;
                else
                    eventsDictionary.Add(@event.Id, @event);
                
                if (ticketType is not null)
                    @event.TicketTypes.Add(ticketType);

                return @event;
            },
            request,
            splitOn: nameof(TicketTypeResponse.TicketTypeId));

        if (!eventsDictionary.TryGetValue(request.EventId, out var eventResponse))
            return Result.Failure<EventResponse>(EventErrors.NotFound(request.EventId));

        return eventResponse;
    }
}