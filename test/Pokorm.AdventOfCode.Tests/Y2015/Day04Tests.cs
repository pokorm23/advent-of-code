using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day04Tests : DayTestBase
{
    public Day04Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne_1()
    {
        var day = new Day04();

        var result = day.Solve("abcdef");

        Assert.Equal(609043, result);
    }

    [Fact]
    public void SampleOne_2()
    {
        var day = new Day04();

        var result = day.Solve("pqrstuv");

        Assert.Equal(1048970, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day04();

        var result = day.Solve(TextForDay(day));

        Assert.Equal(254575, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day04();

        var result = day.SolveBonus(TextForDay(day));

        Assert.Equal(1038736, result);
    }
}
