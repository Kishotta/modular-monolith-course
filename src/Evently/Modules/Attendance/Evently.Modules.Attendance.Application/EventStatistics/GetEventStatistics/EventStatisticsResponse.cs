namespace Evently.Modules.Attendance.Application.EventStatistics.GetEventStatistics;

public record EventStatisticsResponse(
    Guid EventId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    int TicketsSold,
    int AttendeesCheckedIn,
    List<string> DuplicateCheckInTickets,
    List<string> InvalidCheckInTickets);