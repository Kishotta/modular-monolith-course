using Evently.Api;
using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
});

var databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
var cacheConnectionString = builder.Configuration.GetConnectionString("Cache")!;

builder.Services
    .AddApplication([
        Evently.Modules.Events.Application.AssemblyReference.Assembly
    ]) 
    .AddInfrastructure(
        databaseConnectionString,
        cacheConnectionString);

builder.Configuration.AddModuleConfiguration([
    "events"
]);

builder.Services
    .AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(cacheConnectionString);

builder.Services.AddEventsModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.ApplyMigrations();
}

app.MapEndpoints();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

await app.RunAsync();
