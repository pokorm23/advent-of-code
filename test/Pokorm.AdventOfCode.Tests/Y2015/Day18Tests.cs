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
                """), 4, false);

        Assert.Equal(4, result);
    }

    [Fact]
    public void PartOne_F()
    {
        using var _ = logger.BeginScope(XUnitFormattingState.NoColor);

        var day = new Day18(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(821, result);
    }

    [Fact]
    public void PartTwo_1()
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
                """), 5, true);

        Assert.Equal(17, result);
    }

    [Fact]
    public void PartTwo_F()
    {
        using var _ = logger.BeginScope(XUnitFormattingState.NoColor);

        var day = new Day18(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(886, result);
    }
}
