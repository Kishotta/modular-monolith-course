using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Users.Infrastructure.Users;

internal sealed class UserRepository(UsersDbContext db) : IUserRepository
{
    public async Task<User?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await db.Users.SingleOrDefaultAsync(user => user.Id == userId, cancellationToken);
    }

    public void Insert(User user)
    {
        foreach (var role in user.Roles)
        {
            db.Attach(role);
        }

        db.Users.Add(user);
    }
}