using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day19Tests(ILogger<Day19> logger) : DayTestBase
{
    [Fact]
    public void PartOne_1()
    {
        var day = new Day19(logger);

        var result = day.Solve(LinesFromSample(
            """
            r, wr, b, g, bwu, rb, gb, br

            brwrr
            bggr
            gbbr
            rrbgbr
            ubwu
            bwurrg
            brgr
            bbrgwb
            """));

        Assert.Equal(6, result);
    }

    [Fact]
    public void PartOne_F()
    {
        var day = new Day19(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(358, result);
    }

    [Fact]
    public void PartTwo_1()
    {
        var day = new Day19(logger);

        var result = day.SolveBonus(LinesFromSample(
            """
            r, wr, b, g, bwu, rb, gb, br

            brwrr
            bggr
            gbbr
            rrbgbr
            ubwu
            bwurrg
            brgr
            bbrgwb
            """));

        Assert.True(16 == result);
    }

    [Fact]
    public void PartTwo_F()
    {
        var day = new Day19(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(600639829400603, result);
    }
}
