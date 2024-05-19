using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using Evently.Modules.Events.PublicApi;
using MediatR;

namespace Evently.Modules.Events.Infrastructure.PublicApi;

internal sealed class EventsApi(ISender sender) : IEventsApi
{
    public async Task<TicketTypeResponse?> GetTicketTypeAsync(
        Guid ticketTypeId, 
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetTicketTypeQuery(ticketTypeId), cancellationToken);
        
        if (result.IsFailure) return null;

        return new TicketTypeResponse(
            result.Value.Id,
            result.Value.EventId,
            result.Value.Name,
            result.Value.Price,
            result.Value.Currency,
            result.Value.Quantity);
    }
}