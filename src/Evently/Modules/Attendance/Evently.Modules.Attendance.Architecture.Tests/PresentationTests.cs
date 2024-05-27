using System.Reflection;
using Evently.Common.ModuleArchitecture.Tests;

namespace Evently.Modules.Attendance.Architecture.Tests;

public class PresentationTests : CommonPresentationTests
{
    private static readonly Assembly PresentationAssembly = Presentation.AssemblyReference.Assembly;
    
    [Fact]
    public void IntegrationEventHandler_Should_BeSealed()
    {
        AssertIntegrationEventHandlerShouldBeSealed(PresentationAssembly);
    }
    
    [Fact]
    public void IntegrationEventHandler_ShouldHave_NameEndingWith_IntegrationEventHandler()
    {
        AssertIntegrationEventHandlerShouldHaveNameEndingWithIntegrationEventHandler(PresentationAssembly);
    }
}