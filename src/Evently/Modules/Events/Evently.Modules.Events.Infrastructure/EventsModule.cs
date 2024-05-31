using Evently.Common.Application.EventBus;
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
using Evently.Modules.Events.Infrastructure.Inbox;
using Evently.Modules.Events.Infrastructure.Outbox;
using Evently.Modules.Events.Infrastructure.TicketTypes;
using Evently.Modules.Events.Presentation.Events.CancelEventSaga;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainEventHandlers();
        services.AddIntegrationEventHandlers();

        services
            .AddInfrastructure(configuration)
            .AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDatabase(configuration)
            .AddOutbox(configuration)
            .AddInbox(configuration);

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<EventsDbContext>(Postgres.StandardOptions(configuration, Schemas.Events))
            .AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<EventsDbContext>())
            .AddScoped<IEventRepository, EventRepository>()
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<ITicketTypeRepository, TicketTypeRepository>();
    
    private static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<OutboxOptions>(configuration.GetSection("Events:Outbox"))
            .ConfigureOptions<ConfigureProcessOutboxJob>();
    
    private static IServiceCollection AddInbox(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<InboxOptions>(configuration.GetSection("Events:Inbox"))
            .ConfigureOptions<ConfigureProcessInboxJob>();

    public static Action<IRegistrationConfigurator> ConfigureConsumers(string redisConnectionString)
    {
        return registrationConfigurator =>
        {
            registrationConfigurator.AddSagaStateMachine<CancelEventSaga, CancelEventState>()
                .RedisRepository();
            
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
        };
    }

    
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