namespace Evently.Common.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public DateTime OccurredAtUtc { get; init; }
    public DateTime? ProcessedAtUtc { get; init; }
    public string? Error { get; init; }
}