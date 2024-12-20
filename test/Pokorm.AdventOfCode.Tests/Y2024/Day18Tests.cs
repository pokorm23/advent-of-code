using Microsoft.Extensions.Logging.Abstractions;
using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day18Tests(ILogger<Day18> logger) : DayTestBase
{
    [Fact]
    public void PartOne_1()
    {
        var day = new Day18(logger);

        var result = day.SolvePartOne(LinesFromSample(
            """
            5,4
            4,2
            4,5
            3,0
            2,1
            6,3
            2,4
            1,5
            0,6
            3,3
            2,6
            5,1
            1,2
            5,5
            2,5
            6,5
            1,4
            0,4
            6,4
            1,1
            6,1
            1,0
            0,5
            1,6
            2,0
            """), 12, 7);

        Assert.Equal(22, result);
    }

    [Fact]
    public void PartOne_F()
    {
        using var _ = logger.BeginScope(XUnitFormattingState.NoColor);

        var day = new Day18(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(432, result);
    }

    [Fact]
    public void PartTwo_1()
    {
        var day = new Day18(logger);

        var result = day.SolvePartTwo(LinesFromSample(
            """
            5,4
            4,2
            4,5
            3,0
            2,1
            6,3
            2,4
            1,5
            0,6
            3,3
            2,6
            5,1
            1,2
            5,5
            2,5
            6,5
            1,4
            0,4
            6,4
            1,1
            6,1
            1,0
            0,5
            1,6
            2,0
            """), 7);

        Assert.Equal("6,1", result);
    }

    [Fact]
    public void PartTwo_F()
    {
        var day = new Day18(NullLogger<Day18>.Instance);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.NotEqual("38,31", result);
        Assert.Equal("56,27", result);
    }
}
