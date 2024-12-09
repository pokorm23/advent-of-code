namespace Pokorm.AdventOfCode.Tests.Logging;

public interface ITestOutputHelperAccessor
{
    ITestOutputHelper? OutputHelper { get; set; }
}
