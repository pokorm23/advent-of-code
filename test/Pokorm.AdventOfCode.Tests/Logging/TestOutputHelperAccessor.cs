namespace Pokorm.AdventOfCode.Tests.Logging;

internal sealed class TestOutputHelperAccessor(ITestOutputHelper outputHelper) : ITestOutputHelperAccessor
{
    public ITestOutputHelper? OutputHelper { get; set; } = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
}
