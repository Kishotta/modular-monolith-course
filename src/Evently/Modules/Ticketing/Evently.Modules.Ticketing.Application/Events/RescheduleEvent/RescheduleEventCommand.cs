using Evently.Common.Application.Clock;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Events;

namespace Evently.Modules.Ticketing.Application.Events.RescheduleEvent;

public sealed record RescheduleEventCommand(Guid EventId, DateTime StartsAtUtc, DateTime? EndsAtUtc) : ICommand;

internal sealed class RescheduleEventCommandHandler(
    IDateTimeProvider dateTimeProvider,
    IEventRepository events,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RescheduleEventCommand>
{
    public async Task<Result> Handle(RescheduleEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await events.GetAsync(request.EventId, cancellationToken);
        if (@event is null)
            return Result.Failure(EventErrors.NotFound(request.EventId));

        if (request.StartsAtUtc < dateTimeProvider.UtcNow)
            return Result.Failure(EventErrors.StartDateInPast);
        
        @event.Reschedule(request.StartsAtUtc, request.EndsAtUtc);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}