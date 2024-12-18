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
    public void PartTwo_1()
    {
        var day = new Day17(logger);

        var result = day.SolveBonus(LinesFromSample(
            """
            Register A: 2024
            Register B: 0
            Register C: 0

            Program: 0,3,5,4,3,0
            """));

        Assert.Equal(0b_011_100_101_011_000_000, result);
    }

    [Fact]
    public void PartTwo_F_4()
    {
        var day = new Day17(logger);

        var parse = Day17.Parse(LinesFromSample(
            """
            Register A: 0
            Register B: 0
            Register C: 0

            Program: 2,4,1,3,7,5,0,3,1,5,4,1,5,5,3,0
            """));
        
        // N = 16
        // A=000 ->  

        logger.LogInformation(string.Join(Environment.NewLine, parse.Program.GetFormatLines()));

        var c = RunProgramWithARegistry(4, parse.Program);
        
        logger.LogInformation(c.ToString());
    }

    [Fact]
    public void PartTwo_F_FindA()
    {
        var day = new Day17(logger);

        var result = day.SolveBonus2(LinesForDay(day));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo_F()
    {
        var day = new Day17(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }
}
