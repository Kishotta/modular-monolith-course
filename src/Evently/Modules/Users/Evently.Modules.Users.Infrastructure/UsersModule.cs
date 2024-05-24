using Evently.Common.Application.Authorization;
using Evently.Common.Infrastructure.Auditing;
using Evently.Common.Infrastructure.Database;
using Evently.Common.Infrastructure.Outbox;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Application.Abstractions.Identity;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure.Authorization;
using Evently.Modules.Users.Infrastructure.Database;
using Evently.Modules.Users.Infrastructure.Identity;
using Evently.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Evently.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);
        services.AddInfrastructure(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPermissionService, PermissionService>();
        
        services.Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"));
        services.AddTransient<KeyCloakAuthDelegatingHandler>();
        services.AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
        {
            var keyCloakOptions = serviceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
            httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl); 
        })
        .AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

        services.AddTransient<IIdentityProviderService, KeyCloakIdentityProviderService>();
        
        var databaseConnectionString = configuration.GetConnectionString("Database")!;
        
        services.AddDbContext<UsersDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(databaseConnectionString, npgSqlOptions =>
                {
                    npgSqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events);
                }).UseSnakeCaseNamingConvention()
                .AddInterceptors(
                    serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>(),
                    serviceProvider.GetRequiredService<WriteAuditLogInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<UsersDbContext>());
        
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}