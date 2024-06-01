using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.Application.Users.RegisterUser;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Evently.Modules.Users.IntegrationTests.Users;

public class GetUserTests(IntegrationTestWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await Sender.Send(new GetUserQuery(userId));

        // Assert
        result.Error.Should().Be(UserErrors.NotFound(userId));
    }

    [Fact]
    public async Task Should_ReturnUser_WhenUserExists()
    {
        // Arrange
        var result = await Sender.Send(new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(),
            Faker.Name.FirstName(),
            Faker.Name.LastName()));
        var userId = result.Value.Id;

        // Act
        var userResult = await Sender.Send(new GetUserQuery(userId));

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.Value.Should().NotBeNull();

    }
}