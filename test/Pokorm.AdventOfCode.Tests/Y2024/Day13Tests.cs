using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day13Tests(ILogger<Day13> logger) : DayTestBase
{
    [Fact]
    public void PartOne_Sample0()
    {
        var day = new Day13(logger);

        var result = day.Solve(LinesFromSample(
            """
            Button A: X+10, Y+20
            Button B: X+4, Y+8
            Prize: X=28, Y=56
            """));

        Assert.Equal(8, result);
    }

    [Fact]
    public void PartOne_Sample()
    {
        var day = new Day13(logger);

        var result = day.Solve(LinesFromSample(
            """
            Button A: X+94, Y+34
            Button B: X+22, Y+67
            Prize: X=8400, Y=5400

            Button A: X+26, Y+66
            Button B: X+67, Y+21
            Prize: X=12748, Y=12176

            Button A: X+17, Y+86
            Button B: X+84, Y+37
            Prize: X=7870, Y=6450

            Button A: X+69, Y+23
            Button B: X+27, Y+71
            Prize: X=18641, Y=10279
            """));

        Assert.Equal(480, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day13(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(33209, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day13(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }
}
