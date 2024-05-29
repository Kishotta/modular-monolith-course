using Evently.Common.Application.Messaging;
using Evently.Common.Infrastructure.Database;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Attendance.Application.Abstractions.Authentication;
using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.Infrastructure.Attendees;
using Evently.Modules.Attendance.Infrastructure.Authentication;
using Evently.Modules.Attendance.Infrastructure.Database;
using Evently.Modules.Attendance.Infrastructure.Events;
using Evently.Modules.Attendance.Infrastructure.Tickets;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Modules.Attendance.Infrastructure;

public static class AttendanceModule
{
    public static IServiceCollection AddAttendanceModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainEventHandlers();
        
        services
            .AddInfrastructure(configuration)
            .AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddScoped<IAttendanceContext, AttendanceContext>()
            .AddDatabase(configuration);
    
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<AttendanceDbContext>(Postgres.StandardOptions(configuration, Schemas.Attendance))
            .AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AttendanceDbContext>())
            .AddScoped<IAttendeeRepository, AttendeeRepository>()
            .AddScoped<IEventRepository, EventRepository>()
            .AddScoped<ITicketRepository, TicketRepository>();
    
    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumers(Presentation.AssemblyReference.Assembly);
    }
    
    private static void AddDomainEventHandlers(this IServiceCollection services) =>
        Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToList()
            .ForEach(services.TryAddScoped);
}