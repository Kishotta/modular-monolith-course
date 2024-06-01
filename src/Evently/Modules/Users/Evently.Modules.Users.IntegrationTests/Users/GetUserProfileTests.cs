using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Evently.Modules.Users.Application.Users;
using Evently.Modules.Users.IntegrationTests.Abstractions;
using Evently.Modules.Users.Presentation.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Evently.Modules.Users.IntegrationTests.Users;

public class GetUserProfileTests(IntegrationTestWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnUnauthorized_WhenAccessTokenNotProvided()
    {
        // Act
        var response = await HttpClient.GetAsync("users/profile");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenUserExists()
    {
        // Arrange
        var accessToken = await RegisterUserAndGetAccessTokenAsync("exists@test.com", Faker.Internet.Password());
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            accessToken);
        
        // Act
        var response = await HttpClient.GetAsync("users/profile");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await response.Content.ReadFromJsonAsync<UserResponse>();
        user.Should().NotBeNull();
    }

    private async Task<string> RegisterUserAndGetAccessTokenAsync(string email, string password)
    {
        var request = new RegisterUser.Request(
            email, 
            password, 
            Faker.Name.FirstName(), 
            Faker.Name.LastName());

        await HttpClient.PostAsJsonAsync("users/register", request);
        
        var accessToken = await GetAccessTokenAsync(request.Email, request.Password);

        return accessToken;
    }
}