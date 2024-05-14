namespace Evently.Modules.Events.Application.Events.SearchEvents;

public sealed record SearchEventsResponse(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<GetEvents.EventResponse> Events);