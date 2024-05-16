using Evently.Modules.Events.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Evently.Modules.Events.Application.Events.RescheduleEvent;

internal sealed class EventRescheduledDomainEventHandler(ILogger<EventRescheduledDomainEventHandler> logger)
    : IDomainEventHandler<EventRescheduledDomainEvent>
{
    public Task Handle(EventRescheduledDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Event rescheduled: {EventId}", notification.EventId);
        
        return Task.CompletedTask;
    }
}