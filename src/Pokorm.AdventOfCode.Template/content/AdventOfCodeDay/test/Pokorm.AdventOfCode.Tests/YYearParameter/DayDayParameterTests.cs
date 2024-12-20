using Pokorm.AdventOfCode.YYearParameter.Days;

namespace Pokorm.AdventOfCode.Tests.YYearParameter;

public class DayDayParameterTests(ILogger<DayDayParameter> logger) : DayTestBase
{
    [Fact]
    public void PartOne_1()
    {
        var day = new DayDayParameter(logger);

#if (NoLines)
        var result = day.Solve("");
#else
        var result = day.Solve(LinesFromSample(
                """
                ...
                """));
#endif

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartOne_F()
    {
        using var _ = logger.BeginScope(XUnitFormattingState.NoColor);

        var day = new DayDayParameter(logger);

#if (NoLines)
        var result = day.Solve(TextForDay(day));
#else
        var result = day.Solve(LinesForDay(day));
#endif

        Assert.Equal(-1, result);
    }

    /*[Fact]
    public void PartTwo_1()
    {
        var day = new DayDayParameter(logger);

#if (NoLines)
        var result = day.SolveBonus("");
#else
        var result = day.SolveBonus(LinesFromSample(
                """
                ...
                """));
#endif

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo_F()
    {
        using var _ = logger.BeginScope(XUnitFormattingState.NoColor);

        var day = new DayDayParameter(logger);

#if (NoLines)
        var result = day.SolveBonus(TextForDay(day));
#else
        var result = day.SolveBonus(LinesForDay(day));
#endif

        Assert.Equal(-1, result);
    }*/
}
