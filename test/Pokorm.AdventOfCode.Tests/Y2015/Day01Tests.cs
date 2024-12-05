using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day01Tests : DayTestBase
{
    public Day01Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne()
    {
        var day = new Day01();

        var result = day.Solve(TextForDay(day));

        Assert.Equal(74, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day01();

        var result = day.SolveBonus(TextForDay(day));

        Assert.Equal(1795, result);
    }
}
