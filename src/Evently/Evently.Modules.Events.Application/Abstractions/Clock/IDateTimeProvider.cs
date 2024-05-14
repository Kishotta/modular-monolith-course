namespace Evently.Modules.Events.Application.Abstractions.Clock;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}