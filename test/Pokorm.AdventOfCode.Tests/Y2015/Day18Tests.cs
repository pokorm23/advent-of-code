using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day18Tests(ILogger<Day18> logger) : DayTestBase
{
    [Fact]
    public void PartOne_1()
    {
        var day = new Day18(logger);

        var result = day.Solve(LinesFromSample(
                """
                .#.#.#
                ...##.
                #....#
                ..#...
                #.#..#
                ####..
                """), 4);

        Assert.Equal(4, result);
    }

    [Fact]
    public void PartOne_F()
    {
        using var _ = logger.BeginScope(XUnitFormattingState.NoColor);

        var day = new Day18(logger);

        var result = day.Solve(LinesForDay(day), 100);

        Assert.Equal(-1, result);
    }

    /*[Fact]
    public void PartTwo_1()
    {
        var day = new Day18(logger);

        var result = day.SolveBonus(LinesFromSample(
                """
                ...
                """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo_F()
    {
        using var _ = logger.BeginScope(XUnitFormattingState.NoColor);

        var day = new Day18(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }*/
}
