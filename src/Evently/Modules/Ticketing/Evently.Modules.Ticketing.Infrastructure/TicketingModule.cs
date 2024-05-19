using Evently.Common.Infrastructure.Database;
using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Infrastructure.Customers;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Evently.Modules.Ticketing.Infrastructure.PublicApi;
using Evently.Modules.Ticketing.PublicApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);
        services.AddInfrastructure(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("Database")!;

        services.AddDbContext<TicketingDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(
                    databaseConnectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Ticketing);
                    })
                .AddInterceptors(serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>())
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<TicketingDbContext>());

        services.AddSingleton<CartService>();

        services.AddScoped<ITicketingApi, TicketingApi>();
        
        return services;
    }
}