using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day01Tests : DayTestBase
{
    public Day01Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = new Day01();

        var result = day.Solve(LinesFromSample("""
                                               3   4
                                               4   3
                                               2   5
                                               1   3
                                               3   9
                                               3   3
                                               """));

        Assert.Equal(11, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day01();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(1660292, result);
    }

    [Fact]
    public void SampleBonus()
    {
        var day = new Day01();

        var result = day.SolveBonus(LinesFromSample(
            """
            3   4
            4   3
            2   5
            1   3
            3   9
            3   3
            """));

        Assert.Equal(31, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day01();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(22776016, result);
    }
}
