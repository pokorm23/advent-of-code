namespace Pokorm.AdventOfCode.Tests.Y2023;

public class Day06Tests : DayTestBase
{
    public Day06Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = CreateDayFromSample(
            """
            Time:      7  15   30
            Distance:  9  40  200
            """);

        var result = day.Solve();

        Assert.Equal(288, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = CreateDay();

        var result = day.Solve();

        Assert.Equal(625968, result);
    }

    [Fact]
    public void SampleTwo()
    {
        var day = CreateDayFromSample(
            """
            Time:      7  15   30
            Distance:  9  40  200
            """);

        var result = day.SolveBonus();

        Assert.Equal(71503, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = CreateDay();

        var result = day.SolveBonus();

        Assert.Equal(43663323, result);
    }
}
