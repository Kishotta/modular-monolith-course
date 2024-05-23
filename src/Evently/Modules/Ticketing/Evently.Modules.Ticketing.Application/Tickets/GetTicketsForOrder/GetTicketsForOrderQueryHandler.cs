using Dapper;
using Evently.Common.Application.Data;

namespace Evently.Modules.Ticketing.Application.Tickets.GetTicketsForOrder;

internal sealed class GetTicketsForOrderQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketsForOrderQuery, IReadOnlyCollection<TicketResponse>>
{
    public async Task<Result<IReadOnlyCollection<TicketResponse>>> Handle(
        GetTicketsForOrderQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(TicketResponse.Id)},
                 customer_id AS {nameof(TicketResponse.CustomerId)},
                 order_id AS {nameof(TicketResponse.OrderId)},
                 event_id AS {nameof(TicketResponse.EventId)},
                 ticket_type_id AS {nameof(TicketResponse.TicketTypeId)},
                 code AS {nameof(TicketResponse.Code)},
                 created_at_utc AS {nameof(TicketResponse.CreatedAtUtc)}
             FROM ticketing.tickets
             WHERE order_id = @OrderId
             """;

        var tickets = (await connection.QueryAsync<TicketResponse>(sql, request)).AsList();

        return tickets;
    }
}