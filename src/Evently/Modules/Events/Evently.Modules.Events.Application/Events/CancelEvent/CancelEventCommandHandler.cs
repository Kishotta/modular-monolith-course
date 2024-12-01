using Evently.Common.Application.Clock;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.CancelEvent;

internal sealed class CancelEventCommandHandler(
    IDateTimeProvider dateTimeProvider,
    IEventRepository events,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelEventCommand, EventResponse>
{
    public async Task<Result<EventResponse>> Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await events.GetAsync(request.EventId, cancellationToken);
        
        if (@event is null)
            return EventErrors.NotFound(request.EventId);
        
        var result = @event.Cancel(dateTimeProvider.UtcNow);

        if (result.IsFailure)
            return result.Error;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (EventResponse)@event;
    }
}