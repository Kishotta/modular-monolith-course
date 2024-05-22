using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public class EventPublishedIntegrationEvent : IntegrationEvent
{
    public Guid EventId { get; init; }
    public string Title { get; init; }
    
    public EventPublishedIntegrationEvent(
        Guid id,
        DateTime occuredAtUtc,
        Guid eventId,
        string title) : base(id, occuredAtUtc)
    {
        EventId = eventId;
        Title = title;
    }
}