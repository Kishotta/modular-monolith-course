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
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddInfrastructure(configuration)
            .AddEndpoints(Presentation.AssemblyReference.Assembly);

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddScoped<IPermissionService, PermissionService>()
            .AddIdentityProvider(configuration)
            .AddDatabase(configuration);

    private static IServiceCollection AddIdentityProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"))
            .AddTransient<IIdentityProviderService, KeyCloakIdentityProviderService>();

        services.AddTransient<KeyCloakAuthDelegatingHandler>()
            .AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
            {
                var keyCloakOptions = serviceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
                httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<UsersDbContext>(Postgres.StandardOptions(configuration, Schemas.Users))
            .AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<UsersDbContext>())
            .AddScoped<IUserRepository, UserRepository>();
}