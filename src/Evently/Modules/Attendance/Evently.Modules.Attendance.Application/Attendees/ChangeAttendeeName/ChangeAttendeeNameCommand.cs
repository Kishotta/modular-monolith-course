using Evently.Common.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Attendees.ChangeAttendeeName;

public sealed record ChangeAttendeeNameCommand(Guid AttendeeId, string FirstName, string LastName) : ICommand;