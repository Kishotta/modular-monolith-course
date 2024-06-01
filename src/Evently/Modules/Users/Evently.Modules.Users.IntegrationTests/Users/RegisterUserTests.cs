using System.Net;
using System.Net.Http.Json;
using Evently.Modules.Users.IntegrationTests.Abstractions;
using Evently.Modules.Users.Presentation.Users;
using FluentAssertions;

namespace Evently.Modules.Users.IntegrationTests.Users;

public class RegisterUserTests(IntegrationTestWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    public static readonly TheoryData<string, string, string, string> InvalidRequests = new()
    {
        { "", Faker.Internet.Password(), Faker.Name.FirstName(), Faker.Name.LastName() },
        { Faker.Internet.Email(), "", Faker.Name.FirstName(), Faker.Name.LastName() },
        { Faker.Internet.Email(), "12345", Faker.Name.FirstName(), Faker.Name.LastName() },
        { Faker.Internet.Email(), Faker.Internet.Password(), "", Faker.Name.LastName() },
        { Faker.Internet.Email(), Faker.Internet.Password(), Faker.Name.FirstName(), "" },
    };

    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Should_ReturnBadRequest_WhenRequestIsInvalid(
        string email,
        string password,
        string firstName,
        string lastName)
    {
        // Arrange
        var request = new RegisterUser.Request(email, password, firstName, lastName);

        // Act
        var response = await HttpClient.PostAsJsonAsync("users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterUser.Request(
            Faker.Internet.Email(),
            Faker.Internet.Password(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act
        var response = await HttpClient.PostAsJsonAsync("users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Should_ReturnAccessToken_WhenUserIsRegistered()
    {
        // Arrange
        var request = new RegisterUser.Request(
            Faker.Internet.Email(),
            Faker.Internet.Password(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());
        
        await HttpClient.PostAsJsonAsync("users/register", request);
        
        // Act
        var accessToken = await GetAccessTokenAsync(request.Email, request.Password);

        // Assert
        accessToken.Should().NotBeNullOrEmpty();
    }
}