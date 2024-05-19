using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Evently.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Api.Extensions;

public static class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        ApplyMigrations<EventsDbContext>(scope);
        ApplyMigrations<UsersDbContext>(scope);
        ApplyMigrations<TicketingDbContext>(scope);
    }

    private static void ApplyMigrations<TDbContext>(IServiceScope scope)
        where TDbContext : DbContext
    {
        using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
        
        context.Database.Migrate();
    }
    
}