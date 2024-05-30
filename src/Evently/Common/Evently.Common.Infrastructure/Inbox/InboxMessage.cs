namespace Evently.Common.Infrastructure.Inbox;

public sealed class InboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public DateTime OccuredAtUtc { get; init; }
    public DateTime? ProcessedAtUtc { get; init; }
    public string? Error { get; init; }
}