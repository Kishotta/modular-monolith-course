using Evently.Common.Application.Clock;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Tickets;
using Microsoft.Extensions.Logging;

namespace Evently.Modules.Attendance.Application.Attendees.CheckInAttendee;

internal sealed class CheckInAttendeeCommandCommandHandler(
    IAttendeeRepository attendeeRepository,
    ITicketRepository ticketRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork,
    ILogger<CheckInAttendeeCommandCommandHandler> logger)
    : ICommandHandler<CheckInAttendeeCommand>
{
    public async Task<Result> Handle(CheckInAttendeeCommand request, CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetAsync(request.AttendeeId, cancellationToken);
        if (attendee is null)
            return Result.Failure(AttendeeErrors.NotFound(request.AttendeeId));

        var ticket = await ticketRepository.GetAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            return Result.Failure(TicketErrors.NotFound);

        var result = attendee.CheckIn(ticket, dateTimeProvider.UtcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning(
                "Check in failed: {AttendeeId}, {TicketId}, {@Error}",
                attendee.Id,
                ticket.Id,
                result.Error);
        }

        return result;
    }
}