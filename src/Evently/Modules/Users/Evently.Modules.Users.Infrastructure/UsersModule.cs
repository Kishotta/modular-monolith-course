using Evently.Common.Infrastructure.Database;
using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure.Database;
using Evently.Modules.Users.Infrastructure.PublicApi;
using Evently.Modules.Users.Infrastructure.Users;
using Evently.Modules.Users.Presentation;
using Evently.Modules.Users.PublicApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEndpoints(AssemblyReference.Assembly);
        services.AddInfrastructure(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString("Database")!;
        
        services.AddDbContext<UsersDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(databaseConnectionString, npgSqlOptions =>
                {
                    npgSqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events);
                }).UseSnakeCaseNamingConvention()
                .AddInterceptors(serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<UsersDbContext>());
        
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUsersApi, UsersApi>();

        return services;
    }
}