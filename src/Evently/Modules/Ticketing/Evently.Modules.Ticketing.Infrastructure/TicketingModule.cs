using Evently.Common.Application.Messaging;
using Evently.Common.Infrastructure.Database;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Application.Abstractions.Payments;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments;
using Evently.Modules.Ticketing.Domain.Tickets;
using Evently.Modules.Ticketing.Infrastructure.Customers;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Evently.Modules.Ticketing.Infrastructure.Events;
using Evently.Modules.Ticketing.Infrastructure.Orders;
using Evently.Modules.Ticketing.Infrastructure.Outbox;
using Evently.Modules.Ticketing.Infrastructure.Payments;
using Evently.Modules.Ticketing.Infrastructure.Tickets;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainEventHandlers();

        services
            .AddInfrastructure(configuration)
            .AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static IServiceCollection
        AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddSingleton<CartService>()
            .AddSingleton<IPaymentService, FakePaymentService>()
            .AddDatabase(configuration);

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<TicketingDbContext>(Postgres.StandardOptions(configuration, Schemas.Ticketing))
            .AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<TicketingDbContext>())
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IEventRepository, EventRepository>()
            .AddScoped<ITicketTypeRepository, TicketTypeRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<ITicketRepository, TicketRepository>()
            .AddScoped<IPaymentRepository, PaymentRepository>();

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumers(Presentation.AssemblyReference.Assembly);
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

                var closedIdempotentHandlerType =
                    typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEventType);

                services.Decorate(domainEventHandlerType, closedIdempotentHandlerType);
            });
}