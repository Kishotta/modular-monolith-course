using System.Reflection;
using NetArchTest.Rules;

namespace Evently.Common.ModuleArchitecture.Tests;

public abstract class CommonLayerTests
{
    protected static void AssertLayerDoesNotHaveDependencyOnAnotherLayer(Assembly layerAssembly, Assembly anotherLayerAssembly)
    {
        Types.InAssembly(layerAssembly)
            .Should()
            .NotHaveDependencyOn(anotherLayerAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }
}