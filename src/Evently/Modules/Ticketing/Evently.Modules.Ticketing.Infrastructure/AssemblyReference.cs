using System.Reflection;

namespace Evently.Modules.Ticketing.Infrastructure;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}