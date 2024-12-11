using Microsoft.Extensions.Logging;
using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day11Tests : DayTestBase
{
    private readonly ILogger<Day11> logger;

    public Day11Tests(ILogger<Day11> logger) => this.logger = logger;

    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day11(this.logger);

        var result = day.SolveIterations("0 1 10 99 999", 1);

        Assert.Equal(7, result.Stones.Count);
        Assert.Equal("1202410992021976", result.Text);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day11(this.logger);

        var input = "125 17";

        var result = day.SolveIterations(input, 1);

        Assert.Equal(3, result.Stones.Count);

        result = day.SolveIterations(input, 2);

        Assert.Equal(4, result.Stones.Count);

        result = day.SolveIterations(input, 3);

        Assert.Equal(5, result.Stones.Count);

        result = day.SolveIterations(input, 4);

        Assert.Equal(9, result.Stones.Count);

        result = day.SolveIterations(input, 5);

        Assert.Equal(13, result.Stones.Count);

        result = day.SolveIterations(input, 6);

        Assert.Equal(22, result.Stones.Count);

        result = day.SolveIterations(input, 25);

        Assert.Equal(55312, result.Stones.Count);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day11(this.logger);

        var result = day.Solve(TextForDay(day));

        Assert.Equal(218079, result);
    }

    /*[Fact]
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
    }*/
}
