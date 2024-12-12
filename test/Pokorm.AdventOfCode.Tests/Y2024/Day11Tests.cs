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

        // 1 2024 1 0 9 9 2021976
        Assert.Equal(7, result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day11(this.logger);

        var input = "125 17";

        var result = day.SolveIterations(input, 1);

        Assert.Equal(3, result);

        result = day.SolveIterations(input, 2);

        Assert.Equal(4, result);

        result = day.SolveIterations(input, 3);

        Assert.Equal(5, result);

        result = day.SolveIterations(input, 4);

        Assert.Equal(9, result);

        result = day.SolveIterations(input, 5);

        Assert.Equal(13, result);

        result = day.SolveIterations(input, 6);

        Assert.Equal(22, result);

        result = day.SolveIterations(input, 25);

        Assert.Equal(55312, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day11(this.logger);

        var result = day.Solve(TextForDay(day));

        Assert.Equal(218079, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day11(this.logger);

        var result = day.SolveBonus(TextForDay(day));

        Assert.Equal(259755538429618, result);
    }
}
