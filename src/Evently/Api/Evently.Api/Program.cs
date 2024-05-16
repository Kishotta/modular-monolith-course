using Evently.Api.Extensions;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Users.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();

builder.Configuration.AddModuleConfiguration([
    "events",
    "users"
]);

var databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
var cacheConnectionString = builder.Configuration.GetConnectionString("Cache")!;

builder.Services
    .AddExceptionHandling()
    .AddOpenApi()
    .AddModules(
        databaseConnectionString, 
        cacheConnectionString,
        Evently.Modules.Events.Application.AssemblyReference.Assembly,
        Evently.Modules.Users.Application.AssemblyReference.Assembly);


builder.Services
    .AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(cacheConnectionString);

builder.Services
    .AddEventsModule(builder.Configuration)
    .AddUsersModule(builder.Configuration);

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
