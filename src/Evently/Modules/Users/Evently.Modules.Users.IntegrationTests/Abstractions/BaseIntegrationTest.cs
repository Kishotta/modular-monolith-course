using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Bogus;
using Evently.Modules.Users.Infrastructure.Identity;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Evently.Modules.Users.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public class BaseIntegrationTest : IDisposable
{
    protected static readonly Faker Faker = new();
    private readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly HttpClient HttpClient;
    private readonly KeyCloakOptions _options;
    
    protected BaseIntegrationTest(IntegrationTestWebApplicationFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        HttpClient = factory.CreateClient();
        _options = _scope.ServiceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
    }

    protected async Task<string> GetAccessTokenAsync(string email, string password)
    {
        using var client = new HttpClient();
        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _options.PublicClientId),
            new("scope", "openid"),
            new("grant_type", "password"),
            new("username", email),
            new("password", password)
        };

        using var authRequestContent = new FormUrlEncodedContent(authRequestParameters);

        using var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_options.TokenUrl));
        authRequest.Content = authRequestContent;

        using var authorizationResponse = await client.SendAsync(authRequest);
        authorizationResponse.EnsureSuccessStatusCode();

        var authToken =
            await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>();

        return authToken!.AccessToken;
    }

    internal sealed class AuthToken
    {
        [JsonPropertyName("access_token")] 
        public string AccessToken { get; init; } = string.Empty;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) 
        {
            _scope.Dispose();
        }
    }
}