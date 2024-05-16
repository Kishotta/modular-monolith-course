namespace Evently.Modules.Users.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetAsync(Guid userId, CancellationToken cancellationToken = default);

    void Insert(User user);
}