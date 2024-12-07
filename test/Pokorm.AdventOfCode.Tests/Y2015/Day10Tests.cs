using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day10Tests : DayTestBase
{
    public Day10Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample()
    {
        var day = new Day10();

        var result = day.SolveTimes("1", 5);

        Assert.Equal("312211", result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day10();

        var result = day.Solve(TextForDay(day));

        Assert.Equal(492982, result);
    }

    [Fact]
    public void PartTwo_Sample()
    {
        var day = new Day10();

        var result = day.SolveBonus(" ");

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day10();

        var result = day.SolveBonus(TextForDay(day));

        Assert.Equal(-1, result);
    }
}
