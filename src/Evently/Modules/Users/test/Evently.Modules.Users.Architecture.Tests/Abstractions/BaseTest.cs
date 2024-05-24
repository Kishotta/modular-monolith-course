using System.Reflection;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Architecture.Tests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = Application.AssemblyReference.Assembly;
    protected static readonly Assembly DomainAssembly = typeof(User).Assembly;
    protected static readonly Assembly InfrastructureAssembly = Infrastructure.AssemblyReference.Assembly;
    protected static readonly Assembly PresentationAssembly = Presentation.AssemblyReference.Assembly;
}