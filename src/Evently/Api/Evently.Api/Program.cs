using Evently.Api.Extensions;
using Evently.Common.Presentation.Endpoints;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();

builder.Configuration.AddModuleConfiguration([
    "attendance",
    "events",
    "ticketing",
    "users"
]);

var databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
var cacheConnectionString = builder.Configuration.GetConnectionString("Cache")!;

builder.Services
    .AddExceptionHandling()
    .AddOpenApi()
    .AddModules(
        builder.Configuration,
        databaseConnectionString,
        cacheConnectionString);

var keycloakHealthUrl = builder.Configuration.GetValue<string>("KeyCloak:HealthUrl")!;

builder.Services
    .AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(cacheConnectionString)
    .AddUrlGroup(new Uri(keycloakHealthUrl), HttpMethod.Get, "Keycloak");

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

app.UseAuthentication();

app.UseAuthorization();

await app.RunAsync();
