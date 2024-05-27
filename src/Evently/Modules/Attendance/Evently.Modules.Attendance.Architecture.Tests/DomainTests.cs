using System.Reflection;
using Evently.Common.ModuleArchitecture.Tests;
using Evently.Modules.Attendance.Domain.Attendees;

namespace Evently.Modules.Attendance.Architecture.Tests;

public class DomainTests : CommonDomainTests
{
    private static readonly Assembly DomainAssembly = typeof(Attendee).Assembly;
    
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        AssertDomainEventShouldBeSealed(DomainAssembly);
    }

    [Fact]
    public void DomainEvent_ShouldHave_DomainEventPostfix()
    {
        AssertDomainEventShouldHaveDomainEventPostfix(DomainAssembly);
    }
    
    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        AssertEntitiesShouldHavePrivateParameterlessConstructor(DomainAssembly);
    }

    [Fact]
    public void Entities_ShouldOnlyHave_PrivateConstructors()
    {
        AssertEntitiesShouldOnlyHavePrivateConstructors(DomainAssembly);
    }
}