using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day02Tests : DayTestBase
{
    public Day02Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = new Day02();

        var result = day.Solve(LinesFromSample(
            """
            7 6 4 2 1
            1 2 7 8 9
            9 7 6 2 1
            1 3 2 4 5
            8 6 4 4 1
            1 3 6 7 9
            """));

        Assert.Equal(2, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day02();

        var result = day.Solve(LinesForDay(day));

        Assert.True(result > 231);
        Assert.Equal(680, result);
    }

    [Fact]
    public void SampleBonus()
    {
        var day = new Day02();

        var result = day.SolveBonus(LinesFromSample(
            """
            7 6 4 2 1
            1 2 7 8 9
            9 7 6 2 1
            1 3 2 4 5
            8 6 4 4 1
            1 3 6 7 9
            """));

        Assert.Equal(4, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day02();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(710, result);
    }
}
