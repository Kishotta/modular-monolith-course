using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.UnitTests.Abstractions;
using FluentAssertions;

namespace Evently.Modules.Events.UnitTests.Events;

public class CategoryTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenCreated()
    {
        // Arrange
        const string name = "Test Category";

        // Act
        var category = Category.Create(name);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<CategoryCreatedDomainEvent>(category);
        domainEvent.CategoryId.Should().Be(category.Id);
    }
    
    [Fact]
    public void Archive_ShouldRaiseDomainEvent_WhenArchived()
    {
        // Arrange
        var category = Category.Create("Test Category");

        // Act
        category.Archive();

        // Assert
        var domainEvent = AssertDomainEventWasPublished<CategoryArchivedDomainEvent>(category);
        domainEvent.CategoryId.Should().Be(category.Id);
    }
    
    [Fact]
    public void Archive_ShouldReturnFailureResult_WhenAlreadyArchived()
    {
        // Arrange
        var category = Category.Create("Test Category");
        category.Archive();

        // Act
        var result = category.Archive();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.AlreadyArchived);
    }
    
    [Fact]
    public void ChangeName_ShouldRaiseDomainEvent_WhenNameChanged()
    {
        // Arrange
        var category = Category.Create("Test Category");
        const string newName = "New Test Category";

        // Act
        category.ChangeName(newName);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<CategoryNameChangedDomainEvent>(category);
        domainEvent.CategoryId.Should().Be(category.Id);
        domainEvent.Name.Should().Be(newName);
    }
}