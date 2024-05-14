using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastructure.Database;

namespace Evently.Modules.Events.Infrastructure.TicketTypes;

public class TicketTypeRepository(EventsDbContext db) : ITicketTypeRepository
{
    public async Task<TicketType?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await db.TicketTypes.SingleOrDefaultAsync(ticketType => ticketType.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await db.TicketTypes.AnyAsync(ticketType => ticketType.EventId == eventId, cancellationToken);
    }

    public void Insert(TicketType ticketType)
    {
        db.TicketTypes.Add(ticketType);
    }
}