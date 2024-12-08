using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day08Tests : DayTestBase
{
    public Day08Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day08();

        var result = day.Solve(LinesFromSample(
            """
            ..........
            ..........
            ..........
            ....a.....
            ........a.
            .....a....
            ..........
            ......A...
            ..........
            ..........
            """));

        Assert.Equal(4, result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day08();

        var result = day.Solve(LinesFromSample(
            """
            ............
            ........0...
            .....0......
            .......0....
            ....0.......
            ......A.....
            ............
            ............
            ........A...
            .........A..
            ............
            ............
            """));

        Assert.Equal(14, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day08();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(265, result);
    }

    [Fact]
    public void PartTwo_Sample()
    {
        var day = new Day08();

        var result = day.SolveBonus(LinesFromSample(
            """
            ...
            """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day08();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }
}
