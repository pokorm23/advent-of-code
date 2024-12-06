using Pokorm.AdventOfCode.YYearParameter.Days;

namespace Pokorm.AdventOfCode.Tests.YYearParameter;

public class DayDayParameterTests : DayTestBase
{
    public DayDayParameterTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample()
    {
        var day = new DayDayParameter();

#if (NoLines)
        var result = day.Solve(
                """
                ...
                """);
#else
        var result = day.Solve(LinesFromSample(
                """
                ...
                """));
#endif

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new DayDayParameter();

#if (NoLines)
        var result = day.Solve(TextForDay(day));
#else
        var result = day.Solve(LinesForDay(day));
#endif

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo_Sample()
    {
        var day = new DayDayParameter();

#if (NoLines)
        var result = day.SolveBonus(
                """
                ...
                """);
#else
        var result = day.SolveBonus(LinesFromSample(
                """
                ...
                """));
#endif

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new DayDayParameter();

#if (NoLines)
        var result = day.SolveBonus(TextForDay(day));
#else
        var result = day.SolveBonus(LinesForDay(day));
#endif

        Assert.Equal(-1, result);
    }
}
