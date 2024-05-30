using Evently.Api.Middleware;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Modules.Attendance.Infrastructure;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Modules.Users.Infrastructure;

namespace Evently.Api.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        
        return services;
    }
    
    internal static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
        });
        
        return services;
    }
    
    internal static IServiceCollection AddModules(
        this IServiceCollection services,
        IConfiguration configuration,
        string databaseConnectionString,
        string cacheConnectionString)
    {
        services
            .AddApplication([
                Modules.Attendance.Application.AssemblyReference.Assembly,
                Modules.Events.Application.AssemblyReference.Assembly,
                Modules.Ticketing.Application.AssemblyReference.Assembly,
                Modules.Users.Application.AssemblyReference.Assembly
            ]) 
            .AddInfrastructure(
                [
                    AttendanceModule.ConfigureConsumers,
                    EventsModule.ConfigureConsumers,
                    TicketingModule.ConfigureConsumers,
                    UsersModule.ConfigureConsumers
                ],
                databaseConnectionString,
                cacheConnectionString);

        services.AddAttendanceModule(configuration)
            .AddEventsModule(configuration)
            .AddTicketingModule(configuration)
            .AddUsersModule(configuration);
        
        return services;
    }
}