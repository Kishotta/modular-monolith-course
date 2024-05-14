using Evently.Common.Application.Clock;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.RescheduleEvent;

internal sealed class RescheduleEventCommandHandler(
    IDateTimeProvider dateTimeProvider,
    IEventRepository events, 
    IUnitOfWork unitOfWork) : ICommandHandler<RescheduleEventCommand, EventResponse>
{
    public async Task<Result<EventResponse>> Handle(RescheduleEventCommand request, CancellationToken cancellationToken)
    {
        if (request.StartsAtUtc < dateTimeProvider.UtcNow)
            return Result.Failure<EventResponse>(EventErrors.StartDateInPast);
        
        var @event = await events.GetAsync(request.EventId, cancellationToken);

        if (@event is null)
            return Result.Failure<EventResponse>(EventErrors.NotFound(request.EventId));

        var result = @event.Reschedule(request.StartsAtUtc, request.EndsAtUtc);

        if (result.IsFailure)
            return Result.Failure<EventResponse>(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return (EventResponse)@event;
    }
}