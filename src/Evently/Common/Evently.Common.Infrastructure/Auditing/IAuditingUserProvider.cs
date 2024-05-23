namespace Evently.Common.Infrastructure.Auditing;

public interface IAuditingUserProvider
{
    string GetUserId();
}