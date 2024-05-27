using System.Reflection;
using Evently.Architecture.Tests.Abstractions;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Users.Domain.Users;
using NetArchTest.Rules;

namespace Evently.Architecture.Tests.Layers;

public class ModuleTests : BaseTest
{
    [Fact]
    public void AttendanceModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [EventsNamespace, TicketingNamespace, UsersNamespace]; 
        string[] integrationEventsModules =
        [
            EventsIntegrationEventsNamespace,
            TicketingIntegrationEventsNamespace,
            UsersIntegrationEventsNamespace
        ];

        List<Assembly> attendanceAssemblies =
        [
            typeof(Attendee).Assembly,
            Modules.Attendance.Application.AssemblyReference.Assembly,
            Modules.Attendance.Presentation.AssemblyReference.Assembly,
            Modules.Attendance.Infrastructure.AssemblyReference.Assembly,
        ];
        
        Types.InAssemblies(attendanceAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void EventsModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [AttendanceNamespace, TicketingNamespace, UsersNamespace]; 
        string[] integrationEventsModules =
        [
            AttendanceIntegrationEventsNamespace,
            TicketingIntegrationEventsNamespace,
            UsersIntegrationEventsNamespace
        ];

        List<Assembly> eventsAssemblies =
        [
            typeof(Event).Assembly,
            Modules.Events.Application.AssemblyReference.Assembly,
            Modules.Events.Presentation.AssemblyReference.Assembly,
            Modules.Events.Infrastructure.AssemblyReference.Assembly,
        ];
        
        Types.InAssemblies(eventsAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void TicketingModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [AttendanceNamespace, EventsNamespace, UsersNamespace]; 
        string[] integrationEventsModules =
        [
            AttendanceIntegrationEventsNamespace,
            EventsIntegrationEventsNamespace,
            UsersIntegrationEventsNamespace
        ];

        List<Assembly> ticketingAssemblies =
        [
            typeof(Order).Assembly,
            Modules.Ticketing.Application.AssemblyReference.Assembly,
            Modules.Ticketing.Presentation.AssemblyReference.Assembly,
            Modules.Ticketing.Infrastructure.AssemblyReference.Assembly,
        ];
        
        Types.InAssemblies(ticketingAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }
    
    [Fact]
    public void UsersModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [AttendanceNamespace, EventsNamespace, TicketingNamespace]; 
        string[] integrationEventsModules =
        [
            AttendanceIntegrationEventsNamespace,
            EventsIntegrationEventsNamespace,
            TicketingIntegrationEventsNamespace
        ];

        List<Assembly> usersAssemblies =
        [
            typeof(User).Assembly,
            Modules.Users.Application.AssemblyReference.Assembly,
            Modules.Users.Presentation.AssemblyReference.Assembly,
            Modules.Users.Infrastructure.AssemblyReference.Assembly,
        ];
        
        Types.InAssemblies(usersAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }
}