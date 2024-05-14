using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Domain;

namespace Evently.Modules.Events.Application.TicketTypes.GetTicketTypes;

internal sealed class GetTicketTypesQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketTypesQuery, IReadOnlyCollection<TicketTypeResponse>>
{
    public async Task<Result<IReadOnlyCollection<TicketTypeResponse>>> Handle(GetTicketTypesQuery request, CancellationToken cancellationToken)
    {
        await using var dbConnection = await dbConnectionFactory.OpenConnectionAsync();
        
        const string sql =
            $"""
             SELECT
                id AS {nameof(TicketTypeResponse.Id)},
                event_id AS {nameof(TicketTypeResponse.EventId)},
                name AS {nameof(TicketTypeResponse.Name)},
                price AS {nameof(TicketTypeResponse.Price)},
                currency AS {nameof(TicketTypeResponse.Currency)},
                quantity AS {nameof(TicketTypeResponse.Quantity)}
             FROM events.ticket_types
             WHERE event_id = @EventId
             """;

        var ticketTypes = (await dbConnection.QueryAsync<TicketTypeResponse>(sql, request)).AsList();

        return ticketTypes;
    }
}