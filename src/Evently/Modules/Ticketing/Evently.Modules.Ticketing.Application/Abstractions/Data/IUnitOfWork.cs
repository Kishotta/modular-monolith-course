namespace Evently.Modules.Ticketing.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}