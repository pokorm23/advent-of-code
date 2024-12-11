using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day11Tests : DayTestBase
{
    public Day11Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample()
    {
        var day = new Day11();

        var result = day.Solve(LinesFromSample(
                """
                ...
                """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day11();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo_Sample()
    {
        var day = new Day11();

        var result = day.SolveBonus(LinesFromSample(
                """
                ...
                """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day11();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }
}