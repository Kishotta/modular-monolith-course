using Bogus;
using Evently.Common.Domain;

namespace Evently.Modules.Events.UnitTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Faker Faker = new();

    public static TDomainEvent AssertDomainEventWasPublished<TDomainEvent>(Entity entity)
        where TDomainEvent : IDomainEvent
    {
        var domainEvent = entity.GetDomainEvents().OfType<TDomainEvent>().SingleOrDefault();
        
        if(domainEvent is null)
            throw new Exception($"{typeof(TDomainEvent).Name} was not published");

        return domainEvent;
    }
}