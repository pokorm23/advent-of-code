using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day07Tests : DayTestBase
{
    public Day07Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample()
    {
        var day = new Day07();

        var result = day.Solve(LinesFromSample("""
                                               123 -> x
                                               456 -> y
                                               x AND y -> d
                                               x OR y -> e
                                               x LSHIFT 2 -> f
                                               y RSHIFT 2 -> g
                                               NOT x -> h
                                               NOT y -> a
                                               """));

        Assert.Equal(65079, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day07();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(3176, result);
    }
}
