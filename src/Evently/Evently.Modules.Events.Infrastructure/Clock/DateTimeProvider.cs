using Evently.Modules.Events.Application.Abstractions.Clock;

namespace Evently.Modules.Events.Infrastructure.Clock;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}