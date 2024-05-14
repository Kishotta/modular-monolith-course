namespace Evently.Modules.Events.Application.Events.PublishEvent;

public record PublishEventCommand(Guid EventId) : ICommand<EventResponse>;