using Pokorm.AdventOfCode.YYearParameter.Days;

namespace Pokorm.AdventOfCode.Tests.YYearParameter;

public class DayDayParameterTests : DayTestBase
{
    public DayDayParameterTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = new DayDayParameter();

        var result = day.Solve(LinesFromSample(
            """
            ...
            """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new DayDayParameter();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void SampleBonus()
    {
        var day = new DayDayParameter();

        var result = day.SolveBonus(LinesFromSample(
            """
            ...
            """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new DayDayParameter();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }
}
