using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day08Tests : DayTestBase
{
    public Day08Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample()
    {
        var day = new Day08();

        var result = day.Solve(LinesFromSample("""
                                               ""
                                               "abc"
                                               "aaa\"aaa"
                                               "\x27"
                                               """));

        Assert.Equal(12, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day08();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(1342, result);
    }

    [Fact]
    public void PartTwo_Sample()
    {
        var day = new Day08();

        var result = day.SolveBonus(LinesFromSample("""
                                               ""
                                               "abc"
                                               "aaa\"aaa"
                                               "\x27"
                                               """));

        Assert.Equal(19, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day08();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(2074, result);
    }
}
