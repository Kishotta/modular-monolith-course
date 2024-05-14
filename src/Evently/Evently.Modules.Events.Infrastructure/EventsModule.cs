using Evently.Modules.Events.Application.Abstractions.Clock;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastructure.Categories;
using Evently.Modules.Events.Infrastructure.Clock;
using Evently.Modules.Events.Infrastructure.Data;
using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Infrastructure.TicketTypes;
using Evently.Modules.Events.Presentation;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        Presentation
            .AssemblyReference
            .Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IEndpoint)))
            .Where(type => type.IsClass)
            .ToList()
            .ForEach(type =>
            {
                var instance = Activator.CreateInstance(type);

                if (instance is IEndpoint endpoint)
                    endpoint.MapEndpoint(app);
            });
    }

    public static IServiceCollection AddEventsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
        });

        services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);

        services.AddInfrastructure(configuration);
        
        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("Database")!;
        
        var npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        services.AddDbContext<EventsDbContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString, npgSqlOptions =>
            {
                npgSqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events);
            }).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<EventsDbContext>());
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();

        return services;
    }
}