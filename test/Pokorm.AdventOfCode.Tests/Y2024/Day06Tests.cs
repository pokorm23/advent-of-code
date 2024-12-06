using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day06Tests : DayTestBase
{
    public Day06Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = new Day06();

        var result = day.Solve(LinesFromSample(
            """
            ....#.....
            .........#
            ..........
            ..#.......
            .......#..
            ..........
            .#..^.....
            ........#.
            #.........
            ......#...
            """));

        Assert.Equal(41, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day06();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(4939, result);
    }

    [Fact]
    public void SampleBonus()
    {
        var day = new Day06();

        var result = day.SolveBonus(LinesFromSample(
            """
            ...
            """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day06();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }
}
