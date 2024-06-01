using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.UnitTests.Abstractions;
using FluentAssertions;

namespace Evently.Modules.Events.UnitTests.Events;

public class TicketTypeTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenTicketTypeCreated()
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
        var name = Faker.Music.Genre();
        var price = Faker.Random.Decimal(1, 100);
        var currency = "USD";
        var quantity = Faker.Random.Decimal(1, 100);

        // Act
        var ticketType = TicketType.Create(@event, name, price, currency, quantity);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<TicketTypeCreatedDomainEvent>(ticketType);
        domainEvent.TicketTypeId.Should().Be(ticketType.Id);
    }
    
    [Fact]
    public void UpdatePrice_ShouldRaiseDomainEvent_WhenPriceChanged()
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
        var name = Faker.Music.Genre();
        var price = Faker.Random.Decimal(1, 100);
        var currency = "USD";
        var quantity = Faker.Random.Decimal(1, 100);
        var ticketType = TicketType.Create(@event, name, price, currency, quantity);

        // Act
        var newPrice = Faker.Random.Decimal(1, 100);
        ticketType.UpdatePrice(newPrice);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<TicketTypePriceChangedDomainEvent>(ticketType);
        domainEvent.TicketTypeId.Should().Be(ticketType.Id);
        domainEvent.Price.Should().Be(newPrice);
    }
}