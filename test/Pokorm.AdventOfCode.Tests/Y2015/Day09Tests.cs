using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day09Tests : DayTestBase
{
    public Day09Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample()
    {
        var day = new Day09();

        var result = day.Solve(LinesFromSample(
                """
                London to Dublin = 464
                London to Belfast = 518
                Dublin to Belfast = 141
                """));

        Assert.Equal(605, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day09();

        var result = day.Solve(LinesForDay(day));

        Assert.True(result < 283);
        Assert.True(result != 30);
        Assert.Equal(141, result);
    }

    [Fact]
    public void PartTwo_Sample()
    {
        var day = new Day09();

        var result = day.SolveBonus(LinesFromSample(
                """
                ...
                """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day09();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }
}
