using Evently.Common.Application.Messaging;
using Evently.Common.Infrastructure.Database;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastructure.Categories;
using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Infrastructure.TicketTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainEventHandlers();
        
        services
            .AddInfrastructure(configuration)
            .AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<EventsDbContext>(Postgres.StandardOptions(configuration, Schemas.Events))
            .AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<EventsDbContext>())
            .AddScoped<IEventRepository, EventRepository>()
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<ITicketTypeRepository, TicketTypeRepository>();

    private static void AddDomainEventHandlers(this IServiceCollection services) =>
        Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToList()
            .ForEach(services.TryAddScoped);
}