using System.Reflection;
using Evently.Common.Application.EventBus;
using NetArchTest.Rules;

namespace Evently.Common.ModuleArchitecture.Tests;

public abstract class CommonPresentationTests
{
    protected static void AssertIntegrationEventHandlerShouldBeSealed(Assembly assembly)
    {
        Types.InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IIntegrationEventHandler<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }
    
    protected static void AssertIntegrationEventHandlerShouldHaveNameEndingWithIntegrationEventHandler(Assembly assembly)
    {
        Types.InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IIntegrationEventHandler<>))
            .Should()
            .HaveNameEndingWith("IntegrationEventHandler")
            .GetResult()
            .ShouldBeSuccessful();
    }
}