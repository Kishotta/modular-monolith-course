namespace Evently.Common.Infrastructure.Auditing;

public class Audit
{
    public Guid Id { get; init; }
    public string UserId { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string TableName { get; init; } = string.Empty;
    public DateTime OccurredAtUtc { get; init; }
    public string PrimaryKey { get; init; } = string.Empty;
    public string? OldValues { get; init; }
    public string? NewValues { get; init; }
    public string? AffectedColumns { get; init; }

    private Audit() { }
    
    public static Audit Create(
        string userId,
        string auditType,
        string tableName,
        DateTime occurredAtUtc,
        string primaryKey,
        string? oldValues,
        string? newValues,
        string? affectedColumns)
    {
        var audit = new Audit
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = auditType,
            TableName = tableName,
            OccurredAtUtc = occurredAtUtc,
            PrimaryKey = primaryKey,
            OldValues = oldValues,
            NewValues = newValues,
            AffectedColumns = affectedColumns
        };

        return audit;
    }
}