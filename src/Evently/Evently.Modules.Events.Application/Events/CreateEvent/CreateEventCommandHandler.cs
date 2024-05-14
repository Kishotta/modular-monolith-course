using Evently.Common.Application.Clock;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.CreateEvent;

internal sealed class CreateEventCommandHandler(
    IDateTimeProvider dateTimeProvider,
    IEventRepository events,
    ICategoryRepository categories,
    IUnitOfWork unitOfWork) 
    : ICommandHandler<CreateEventCommand, EventResponse>
{
    public async Task<Result<EventResponse>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        if (request.StartsAtUtc < dateTimeProvider.UtcNow)
            return Result.Failure<EventResponse>(EventErrors.StartDateInPast);
        
        var category = await categories.GetAsync(request.CategoryId, cancellationToken);
        
        if (category is null)
            return Result.Failure<EventResponse>(CategoryErrors.NotFound(request.CategoryId));
        
        var result = Event.CreateDraft(
            category,
            request.Title, 
            request.Description, 
            request.Location, 
            request.StartsAtUtc, 
            request.EndsAtUtc);

        if (result.IsFailure)
            return Result.Failure<EventResponse>(result.Error);
        
        events.Insert(result.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success<EventResponse>(result.Value);
    }
}