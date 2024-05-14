using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Categories;

internal sealed class CategoryRepository(EventsDbContext db) : ICategoryRepository
{
    public async Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.Categories.SingleOrDefaultAsync(category => category.Id == id, cancellationToken);
    }

    public void Insert(Category category)
    {
        db.Categories.Add(category);
    }
}