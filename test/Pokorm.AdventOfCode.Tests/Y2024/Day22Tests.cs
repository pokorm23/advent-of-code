using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day22Tests(ILogger<Day22> logger) : DayTestBase
{
    [Fact]
    public void PartOne_1()
    {
        var day = new Day22(logger);

        var result = day.Solve(LinesFromSample(
                """
                1
                10
                100
                2024
                """));

        Assert.Equal(37327623, result);
    }

    [Fact]
    public void PartOne_F()
    {
        var day = new Day22(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(12759339434, result);
    }

    /*[Fact]
    public void PartTwo_1()
    {
        var day = new Day22(logger);

        var result = day.SolveBonus(LinesFromSample(
                """
                ...
                """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo_F()
    {
        var day = new Day22(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }*/
}
