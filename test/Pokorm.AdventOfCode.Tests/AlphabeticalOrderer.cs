using Xunit.Sdk;

namespace Pokorm.AdventOfCode.Tests;

public class AlphabeticalOrderer : ITestCaseOrderer
{
    public const string TypeName = "Pokorm.AdventOfCode.Tests.AlphabeticalOrderer";
    public const string AssemblyName = "Pokorm.AdventOfCode.Tests";

    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase => testCases.OrderBy(testCase => testCase.TestMethod.Method.Name);
}
