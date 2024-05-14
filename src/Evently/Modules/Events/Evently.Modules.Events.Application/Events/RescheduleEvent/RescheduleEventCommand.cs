namespace Evently.Modules.Events.Application.Events.RescheduleEvent;

public sealed record RescheduleEventCommand(
    Guid EventId, 
    DateTime StartsAtUtc, 
    DateTime? EndsAtUtc) : ICommand<EventResponse>;