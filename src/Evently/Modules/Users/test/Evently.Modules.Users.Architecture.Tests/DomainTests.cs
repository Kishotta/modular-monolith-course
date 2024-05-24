using System.Reflection;
using Evently.Common.Domain;
using Evently.Modules.Users.Architecture.Tests.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;

namespace Evently.Modules.Users.Architecture.Tests;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(DomainEvent))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainEvent_ShouldHave_DomainEventPostfix()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(DomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult()
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();
        foreach (var entityType in entityTypes)
        {
            var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            if (!Enumerable.Any(constructors, constructor => constructor.IsPrivate && constructor.GetParameters().Length == 0))
                failingTypes.Add(entityType);
        }

        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void Entities_ShouldOnlyHave_PrivateConstructors()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();
        
        var failingTypes = new List<Type>();
        foreach (var entityType in entityTypes)
        {
            var constructors = entityType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length != 0)
                failingTypes.Add(entityType);
        }

        failingTypes.Should().BeEmpty();
    }
}