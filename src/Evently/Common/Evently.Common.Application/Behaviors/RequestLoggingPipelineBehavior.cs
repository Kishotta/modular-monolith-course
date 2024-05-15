using Evently.Common.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Evently.Common.Application.Behaviors;

internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var moduleName = GetModuleName(typeof(TRequest).FullName!);
        var requestName = typeof(TRequest).Name;

        using (LogContext.PushProperty("Module", moduleName))
        {
            logger.LogInformation("Processing request {RequestName}", requestName);

            var result = await next();

            if (result.IsSuccess)
            {
                logger.LogInformation("Completed request {RequestName}", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Completed request {RequestName} with error", requestName);
                }
            }

            return result;
        }
    }

    // Hack: This relies on the convention for location and naming of module application commands
    /// <summary>
    /// Pulls the module name out of the fully qualified name of the request type.
    /// </summary>
    /// <remarks>
    /// Module commands are conventionally in the form of: "Evently.Modules.[Module].Application..."
    /// </remarks>
    /// <param name="requestName">The fully qualified name of the request type</param>
    /// <returns>The module name</returns>
    private static string GetModuleName(string requestName) => 
        requestName.Split('.')[2];
}