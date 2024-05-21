namespace Evently.Common.Infrastructure.Auditing;

public interface IAuditUserProvider
{
    string GetUserId();
}