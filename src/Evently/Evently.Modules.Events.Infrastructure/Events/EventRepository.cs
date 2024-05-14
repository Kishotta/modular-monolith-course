using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Events;

internal sealed class EventRepository(EventsDbContext db) : IEventRepository
{
    public async Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.Events.SingleOrDefaultAsync(@event => @event.Id == id, cancellationToken);
    }

    public void Insert(Event @event)
    {
        db.Events.Add(@event);
    }
}