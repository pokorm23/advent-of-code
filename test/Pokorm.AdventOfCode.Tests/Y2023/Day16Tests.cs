namespace Pokorm.AdventOfCode.Tests.Y2023;

public class Day16Tests : DayTestBase
{
    public Day16Tests(ITestOutputHelper output) : base(output) { }

    //[Fact]
    public void SampleOne()
    {
        var day = CreateDayFromSample(
            """

            """);

        var result = day.Solve();

        Assert.Equal(6440, result);
    }

    //[Fact]
    public void PartOne()
    {
        var day = CreateDay();

        var result = day.Solve();

        Assert.Equal(249748283, result);
    }

    //[Fact]
    public void SampleTwo()
    {
        var day = CreateDayFromSample(
            """

            """);

        var result = day.SolveBonus();

        Assert.Equal(71503, result);
    }

    //[Fact]
    public void PartTwo()
    {
        var day = CreateDay();

        var result = day.SolveBonus();

        Assert.Equal(43663323, result);
    }
}
