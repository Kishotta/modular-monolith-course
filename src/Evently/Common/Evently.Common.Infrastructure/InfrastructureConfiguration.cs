using Evently.Common.Application.Caching;
using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Infrastructure.Auditing;
using Evently.Common.Infrastructure.Authentication;
using Evently.Common.Infrastructure.Authorization;
using Evently.Common.Infrastructure.Caching;
using Evently.Common.Infrastructure.Clock;
using Evently.Common.Infrastructure.Data;
using Evently.Common.Infrastructure.Outbox;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Quartz;
using StackExchange.Redis;

namespace Evently.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        Action<IRegistrationConfigurator>[] moduleConfigureConsumers,
        string databaseConnectionString,
        string cacheConnectionString)
    {
        services.AddAuthorizationInternal();
        services.AddAuthenticationInternal();
        
        services.AddAuditing();
        
        var npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        
        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddQuartz(configurator =>
        {
            var scheduler = Guid.NewGuid();
            configurator.SchedulerId = $"default-id-{scheduler}";
            configurator.SchedulerName = $"default-name-{scheduler}";
        });
        
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        
        try
        {
            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(cacheConnectionString);
            services.TryAddSingleton(connectionMultiplexer);

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
            });
        }
        catch
        {
            // HACK: Allows application to run without a Redis server. This is useful when, for example, generating a database migration.
            services.AddDistributedMemoryCache();
        }
        
        services.TryAddSingleton<ICacheService, CacheService>();
        services.TryAddSingleton<IEventBus, EventBus.EventBus>();

        services.AddMassTransit(configurator =>
        {
            foreach (var configureConsumer in moduleConfigureConsumers)
            {
                configureConsumer(configurator);
            }
            
            configurator.SetKebabCaseEndpointNameFormatter();
            configurator.UsingInMemory((context, config) =>
            {
                config.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}