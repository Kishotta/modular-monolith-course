using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Application.Events.CreateEvent;

internal sealed class CreateEventCommandHandler(
    IEventRepository events,
    ITicketTypeRepository ticketTypes,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEventCommand>
{
    public async Task<Result> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = Event.Create(
            request.EventId,
            request.Title,
            request.Description,
            request.Location,
            request.StartsAtUtc,
            request.EndsAtUtc);

        events.Insert(@event);

        var eventTicketTypes = request.TicketTypes
            .Select(ticketType => TicketType.Create(
                ticketType.TicketTypeId, 
                ticketType.EventId,
                ticketType.Name,
                ticketType.Price, 
                ticketType.Currency, 
                ticketType.Quantity));
        
        ticketTypes.InsertRange(eventTicketTypes);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}