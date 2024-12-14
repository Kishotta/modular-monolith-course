namespace Evently.Modules.Events.Application.Events.GetEvents;

public sealed record GetEventsQuery(int Page, int PageSize)
    : PaginatedQuery<IReadOnlyCollection<EventResponse>>(Page, PageSize);