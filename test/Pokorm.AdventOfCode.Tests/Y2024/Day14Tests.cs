using Pokorm.AdventOfCode.Helpers;
using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day14Tests(ILogger<Day14> logger) : DayTestBase
{
    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day14(logger);

        var result = day.SolveInGrid(LinesFromSample(
            """
            p=2,4 v=2,-3
            """), new Grid(11, 7), 5);

        Assert.Equal(0, result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day14(logger);

        var result = day.SolveInGrid(LinesFromSample(
            """
            p=0,4 v=3,-3
            p=6,3 v=-1,-3
            p=10,3 v=-1,2
            p=2,0 v=2,-1
            p=0,0 v=1,3
            p=3,0 v=-2,-2
            p=7,6 v=-1,-3
            p=3,0 v=-1,-2
            p=9,3 v=2,3
            p=7,3 v=-1,2
            p=2,4 v=2,-3
            p=9,5 v=-3,-3
            """), new Grid(11,7), 100);

        Assert.Equal(12, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day14(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(224357412, result);
    }

    /*[Fact]
    public void PartTwo_Sample()
    {
        var day = new Day14(logger);

        var result = day.SolveBonus(LinesFromSample(
                """
                ...
                """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day14(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }*/
}
