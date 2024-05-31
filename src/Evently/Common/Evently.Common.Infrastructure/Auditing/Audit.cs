namespace Evently.Common.Infrastructure.Auditing;

public class Audit
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public DateTime OccurredAtUtc { get; set; }
    public string PrimaryKey { get; set; } = string.Empty;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? AffectedColumns { get; set; }

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