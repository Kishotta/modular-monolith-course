using System.Reflection;
using Evently.Common.ModuleArchitecture.Tests;
using Evently.Modules.Attendance.Domain.Attendees;

namespace Evently.Modules.Attendance.Architecture.Tests;

public class LayerTests : CommonLayerTests
{
    private static readonly Assembly ApplicationAssembly = Application.AssemblyReference.Assembly;
    private static readonly Assembly DomainAssembly = typeof(Attendee).Assembly;
    private static readonly Assembly InfrastructureAssembly = Infrastructure.AssemblyReference.Assembly;
    private static readonly Assembly PresentationAssembly = Presentation.AssemblyReference.Assembly;
    
    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
    {
        AssertLayerDoesNotHaveDependencyOnAnotherLayer(
            DomainAssembly, 
            ApplicationAssembly);
    }
    
    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        AssertLayerDoesNotHaveDependencyOnAnotherLayer(
            DomainAssembly, 
            InfrastructureAssembly);
    }
    
    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        AssertLayerDoesNotHaveDependencyOnAnotherLayer(
            ApplicationAssembly, 
            InfrastructureAssembly);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        AssertLayerDoesNotHaveDependencyOnAnotherLayer(
            ApplicationAssembly, 
            PresentationAssembly);
    }
    
    [Fact]
    public void PresentationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        AssertLayerDoesNotHaveDependencyOnAnotherLayer(
            PresentationAssembly, 
            InfrastructureAssembly);
    }
}