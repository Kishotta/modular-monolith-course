using System.Data.Common;

namespace Evently.Modules.Ticketing.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}