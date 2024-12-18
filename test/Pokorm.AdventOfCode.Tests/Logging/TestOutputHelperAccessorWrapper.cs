namespace Pokorm.AdventOfCode.Tests.Logging;

internal sealed class TestOutputHelperAccessorWrapper(Xunit.DependencyInjection.ITestOutputHelperAccessor accessor)
    : Logging.ITestOutputHelperAccessor
{
    ITestOutputHelper? Logging.ITestOutputHelperAccessor.OutputHelper
    {
        get => accessor.Output;
        set => accessor.Output = value;
    }
}
