using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.UnitTests.Abstractions;
using FluentAssertions;

namespace Evently.Modules.Events.UnitTests.Events;

public class EventTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenEndDateProceedsStartDate()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow;
        var endsAtUtc = startsAtUtc.AddMinutes(-1);
        
        // Act
        var result = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            endsAtUtc);

        // Assert
        result.Error.Should().Be(EventErrors.EndDatePrecedesStartDate);
    }
    
    [Fact]
    public void Create_ShouldCreateEvent_WhenValidData()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow;
        
        // Act
        var result = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        var @event = result.Value;
        
        // Assert
        @event.CategoryId.Should().Be(category.Id);
        @event.Title.Should().NotBeNullOrEmpty();
        @event.Description.Should().NotBeNullOrEmpty();
        @event.Location.Should().NotBeNullOrEmpty();
        @event.StartsAtUtc.Should().Be(startsAtUtc);
        @event.EndsAtUtc.Should().BeNull();
        @event.Status.Should().Be(EventStatus.Draft);
    }
    
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenEventCreated()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow;
        
        // Act
        var result = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        var @event = result.Value;
        
        // Assert
        var domainEvent = AssertDomainEventWasPublished<EventCreatedDomainEvent>(@event);
        domainEvent.EventId.Should().Be(@event.Id);
    }
    
    [Fact]
    public void Publish_ShouldReturnFailure_WhenEventAlreadyPublished()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow;
        var @event = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null).Value;
        @event.Publish();
        
        // Act
        var publishResult = @event.Publish();
        
        // Assert
        publishResult.Error.Should().Be(EventErrors.NotDraft);
    }
    
    [Fact]
    public void Publish_ShouldRaiseDomainEvent_WhenEventPublished()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow;
        var @event = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null).Value;
        
        // Act
        @event.Publish();
        
        // Assert
        var domainEvent = AssertDomainEventWasPublished<EventPublishedDomainEvent>(@event);
        domainEvent.EventId.Should().Be(@event.Id);
    }
    
    [Fact]
    public void Reschedule_ShouldReturnFailure_WhenEndDateProceedsStartDate()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow;
        var @event = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null).Value;
        
        // Act
        var rescheduleResult = @event.Reschedule(
            startsAtUtc,
            startsAtUtc.AddMinutes(-1));
        
        // Assert
        rescheduleResult.Error.Should().Be(EventErrors.EndDatePrecedesStartDate);
    }
    
    [Fact]
    public void Reschedule_ShouldUpdateStartsAtAndEndsAt_WhenRescheduled()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow;
        var @event = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null).Value;
        
        var newStartsAtUtc = startsAtUtc.AddHours(1);
        var newEndsAtUtc = newStartsAtUtc.AddHours(1);
        
        // Act
        @event.Reschedule(newStartsAtUtc, newEndsAtUtc);
        
        // Assert
        @event.StartsAtUtc.Should().Be(newStartsAtUtc);
        @event.EndsAtUtc.Should().Be(newEndsAtUtc);
    }
    
    [Fact]
    public void Reschedule_ShouldNotUpdateEndsAt_WhenEndsAtIsNull()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow;
        var @event = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null).Value;
        
        var newStartsAtUtc = startsAtUtc.AddHours(1);
        
        // Act
        @event.Reschedule(newStartsAtUtc, null);
        
        // Assert
        @event.StartsAtUtc.Should().Be(newStartsAtUtc);
        @event.EndsAtUtc.Should().BeNull();
    }
    
    [Fact]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventRescheduled()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow;
        var @event = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null).Value;
        
        var newStartsAtUtc = startsAtUtc.AddHours(1);
        var newEndsAtUtc = newStartsAtUtc.AddHours(1);
        
        // Act
        @event.Reschedule(newStartsAtUtc, newEndsAtUtc);
        
        // Assert
        var domainEvent = AssertDomainEventWasPublished<EventRescheduledDomainEvent>(@event);
        domainEvent.EventId.Should().Be(@event.Id);
    }
    
    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyCancelled()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow;
        var @event = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null).Value;
        @event.Cancel(startsAtUtc);
        
        // Act
        var cancelResult = @event.Cancel(startsAtUtc);
        
        // Assert
        cancelResult.Error.Should().Be(EventErrors.AlreadyCancelled);
    }
    
    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyStarted()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow.AddHours(-1);
        var @event = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null).Value;
        
        // Act
        var cancelResult = @event.Cancel(DateTime.UtcNow);
        
        // Assert
        cancelResult.Error.Should().Be(EventErrors.AlreadyStarted);
    }
    
    [Fact]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventCancelled()
    {
        // Arrange
        var category = Category.Create(Faker.Music.Genre());
        var startsAtUtc = DateTime.UtcNow;
        var @event = Event.CreateDraft(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null).Value;
        
        // Act
        @event.Cancel(startsAtUtc);
        
        // Assert
        var domainEvent = AssertDomainEventWasPublished<EventCancelledDomainEvent>(@event);
        domainEvent.EventId.Should().Be(@event.Id);
    }
}