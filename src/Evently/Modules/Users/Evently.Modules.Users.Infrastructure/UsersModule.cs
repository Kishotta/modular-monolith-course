using Evently.Common.Application.Authorization;
using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
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
using Evently.Modules.Users.Infrastructure.Inbox;
using Evently.Modules.Users.Infrastructure.Outbox;
using Evently.Modules.Users.Infrastructure.Users;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Evently.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainEventHandlers();
        services.AddIntegrationEventHandlers();
        
        services
            .AddInfrastructure(configuration)
            .AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static IServiceCollection
        AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddScoped<IPermissionService, PermissionService>()
            .AddIdentityProvider(configuration)
            .AddDatabase(configuration)
            .AddOutbox(configuration)
            .AddInbox(configuration);

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

    private static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<OutboxOptions>(configuration.GetSection("Users:Outbox"))
            .ConfigureOptions<ConfigureProcessOutboxJob>();
    
    private static IServiceCollection AddInbox(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<InboxOptions>(configuration.GetSection("Users:Inbox"))
            .ConfigureOptions<ConfigureProcessInboxJob>();
    
    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator) =>
        Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToList()
            .ForEach(integrationEventHandlerType =>
            {
                var integrationEventType = integrationEventHandlerType
                    .GetInterfaces()
                    .Single(@interface => @interface.IsGenericType)
                    .GetGenericArguments()
                    .Single();
                
                registrationConfigurator.AddConsumer(typeof(IntegrationEventConsumer<>).MakeGenericType(integrationEventType));
            });

    private static void AddDomainEventHandlers(this IServiceCollection services) =>
        Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToList()
            .ForEach(domainEventHandlerType =>
            {
                services.TryAddScoped(domainEventHandlerType);

                var domainEventType = domainEventHandlerType
                    .GetInterfaces()
                    .Single(@interface => @interface.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentHandlerType = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEventType);
                
                services.Decorate(domainEventHandlerType, closedIdempotentHandlerType);
            });
    
    private static void AddIntegrationEventHandlers(this IServiceCollection services) =>
        Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToList()
            .ForEach(integrationEventHandlerType =>
            {
                services.TryAddScoped(integrationEventHandlerType);

                var integrationEventType = integrationEventHandlerType
                    .GetInterfaces()
                    .Single(@interface => @interface.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentHandlerType = typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEventType);
                
                services.Decorate(integrationEventHandlerType, closedIdempotentHandlerType);
            });
}