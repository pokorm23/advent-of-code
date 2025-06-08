using Pokorm.AdventOfCode.Y2024.Days;
using static Pokorm.AdventOfCode.Y2024.Days.Day17;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day17Tests(ILogger<Day17> logger) : DayTestBase
{
    [Fact]
    public void PartOne_1()
    {
        var day = new Day17(logger);

        var result = day.Solve(LinesFromSample(
            """
            Register A: 729
            Register B: 0
            Register C: 0

            Program: 0,1,5,4,3,0
            """));

        Assert.Equal("4,6,3,5,6,3,5,2,1,0", result);
    }

    [Fact]
    public void PartOne_F()
    {
        var day = new Day17(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal("1,6,7,4,3,0,5,0,6", result);
    }

    [Fact]
    public void PartTwo_1_Naive()
    {
        var day = new Day17(logger);

        var result = day.SolveBonus_Naive(LinesFromSample(
            """
            Register A: 2024
            Register B: 0
            Register C: 0

            Program: 0,3,5,4,3,0
            """));

        Assert.Equal(0b_011_100_101_011_000_000, result);
    }

    [Fact]
    public void PartTwo_1_Recursive()
    {
        var day = new Day17(logger);

        var result = day.SolveBonus_Recursive(LinesFromSample(
            """
            Register A: 2024
            Register B: 0
            Register C: 0

            Program: 0,3,5,4,3,0
            """));

        Assert.Equal(0b_011_100_101_011_000_000, result);
    }

    [Fact]
    public void PartTwo_F()
    {
        var day = new Day17(logger);

        var result = day.SolveBonus_Recursive(LinesForDay(day));

        Assert.Equal(216148338630253, result);
    }
}
