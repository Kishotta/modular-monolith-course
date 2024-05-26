using FluentAssertions;
using NetArchTest.Rules;

namespace Evently.Common.ModuleArchitecture.Tests;

internal static class TestResultExtensions
{
    internal static void ShouldBeSuccessful(this TestResult testResult)
    {
        testResult.FailingTypes?.Should().BeEmpty();
    }
}