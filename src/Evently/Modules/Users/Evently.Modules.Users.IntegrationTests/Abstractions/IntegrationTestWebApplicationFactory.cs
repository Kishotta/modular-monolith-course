using Evently.Modules.Users.Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace Evently.Modules.Users.IntegrationTests.Abstractions;

public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("evently")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    
    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();

    private readonly KeycloakContainer _keycloakContainer = new KeycloakBuilder()
        .WithImage("quay.io/keycloak/keycloak:latest")
        .WithResourceMapping(
            new FileInfo("evently-realm-export.json"),
            new FileInfo("/opt/keycloak/data/import/realm.json"))
        .WithCommand("--import-realm")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:Database", _databaseContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("ConnectionStrings:Cache", _redisContainer.GetConnectionString());

        var keycloakAddress = _keycloakContainer.GetBaseAddress();
        var keycloakRealmUrl = $"{keycloakAddress}/realms/evently";
        
        Environment.SetEnvironmentVariable("Authentication:MetadataAddress", $"{keycloakAddress}/realms/master/.well-known/openid-configuration");
        Environment.SetEnvironmentVariable("Authentication:TokenValidationParameters:ValidIssuer", keycloakRealmUrl);
        
        builder.ConfigureTestServices(services =>
        {
            services.Configure<KeyCloakOptions>(options =>
            {
                options.AdminUrl = $"{keycloakAddress}admin/realms/evently/";
                options.TokenUrl = $"{keycloakRealmUrl}/protocol/openid-connect/token";
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _databaseContainer.StartAsync();
        await _redisContainer.StartAsync();
        await _keycloakContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _databaseContainer.StopAsync();
        await _redisContainer.StopAsync();
        await _keycloakContainer.StopAsync();
    }
}