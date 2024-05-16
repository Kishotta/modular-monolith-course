using System.Reflection;
using Evently.Api.Middleware;
using Evently.Common.Application;
using Evently.Common.Infrastructure;

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
        string databaseConnectionString,
        string cacheConnectionString, 
        params Assembly[] moduleAssemblies)
    {
        services
            .AddApplication(moduleAssemblies) 
            .AddInfrastructure(
                databaseConnectionString,
                cacheConnectionString);
        
        return services;
    }
}