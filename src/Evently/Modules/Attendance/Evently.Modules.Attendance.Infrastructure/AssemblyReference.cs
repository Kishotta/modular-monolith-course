using System.Reflection;

namespace Evently.Modules.Attendance.Infrastructure;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}