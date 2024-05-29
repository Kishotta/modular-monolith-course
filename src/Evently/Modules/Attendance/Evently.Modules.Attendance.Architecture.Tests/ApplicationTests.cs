using System.Reflection;
using Evently.Common.ModuleArchitecture.Tests;
using Evently.Modules.Attendance.Application;

namespace Evently.Modules.Attendance.Architecture.Tests;

public class ApplicationTests : CommonApplicationTests
{
    private static readonly Assembly ApplicationAssembly = AssemblyReference.Assembly;
    
    [Fact]
    public void Commands_Should_BeSealed()
    {
        AssertCommandsShouldBeSealed(ApplicationAssembly);
    }

    [Fact]
    public void Commands_ShouldHave_NameEndingWith_Command()
    {
        AssertCommandsShouldHaveNameEndingWithCommand(ApplicationAssembly);
    }

    [Fact]
    public void CommandHandlers_Should_NotBePublic()
    {
        AssertCommandHandlersShouldNotBePublic(ApplicationAssembly);
    }

    [Fact]
    public void CommandHandlers_Should_BeSealed()
    {
       AssertCommandHandlersShouldBeSealed(ApplicationAssembly);
    }

    [Fact]
    public void CommandHandlers_ShouldHave_NameEndingWith_CommandHandler()
    {
        AssertCommandHandlersShouldHaveNameEndingWithCommandHandler(ApplicationAssembly);
    }
    
    [Fact]
    public void Queries_Should_BeSealed()
    {
        AssertQueriesShouldBeSealed(ApplicationAssembly);
    }
    
    [Fact]
    public void Query_ShouldHave_NameEndingWith_Query()
    {
        AssertQueriesShouldHaveNameEndingWithQuery(ApplicationAssembly);
    }
    
    [Fact]
    public void QueryHandler_Should_NotBePublic()
    {
        AssertQueryHandlersShouldNotBePublic(ApplicationAssembly);
    }
    
    [Fact]
    public void QueryHandler_Should_BeSealed()
    {
        AssertQueryHandlersShouldBeSealed(ApplicationAssembly);
    }
    
    [Fact]
    public void QueryHandler_ShouldHave_NameEndingWith_QueryHandler()
    {
        AssertQueryHandlersShouldHaveNameEndingWithQueryHandler(ApplicationAssembly);
    }
    
    [Fact]
    public void Validator_Should_NotBePublic()
    {
        AssertValidatorsShouldNotBePublic(ApplicationAssembly);
    }
    
    [Fact]
    public void Validator_Should_BeSealed()
    {
        AssertValidatorsShouldBeSealed(ApplicationAssembly);
    }
    
    [Fact]
    public void Validator_ShouldHave_NameEndingWith_Validator()
    {
        AssertValidatorsShouldHaveNameEndingWithValidator(ApplicationAssembly);
    }
    
    [Fact]
    public void DomainEventHandler_Should_NotBePublic()
    {
        AssertDomainEventHandlersShouldNotBePublic(ApplicationAssembly);
    }
    
    [Fact]
    public void DomainEventHandler_Should_BeSealed()
    {
        AssertDomainEventHandlersShouldBeSealed(ApplicationAssembly);
    }
    
    [Fact]
    public void DomainEventHandler_ShouldHave_NameEndingWith_DomainEventHandler()
    {
        AssertDomainEventHandlersShouldHaveNameEndingWithDomainEventHandler(ApplicationAssembly);
    }
}