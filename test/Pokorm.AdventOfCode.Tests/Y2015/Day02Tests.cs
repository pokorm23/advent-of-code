using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day02Tests : DayTestBase
{
    public Day02Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne()
    {
        var day = new Day02();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(1598415, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day02();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(3812909, result);
    }
}
