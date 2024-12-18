using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;

namespace Evently.Modules.Events.Application.Events.PublishEvent;

internal sealed class PublishEventCommandHandler(
    IEventRepository events,
    ITicketTypeRepository ticketTypes,
    IUnitOfWork unitOfWork)
    : ICommandHandler<PublishEventCommand, EventResponse>
{
    public async Task<Result<EventResponse>> Handle(PublishEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await events.GetAsync(request.EventId, cancellationToken);
        
        if (@event is null)
            return EventErrors.NotFound(request.EventId);

        if (!await ticketTypes.ExistsAsync(@event.Id, cancellationToken))
            return EventErrors.NoTicketsFound;

        var result = @event.Publish();
        
        if (result.IsFailure)
            return result.Error;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (EventResponse)@event;
    }
}