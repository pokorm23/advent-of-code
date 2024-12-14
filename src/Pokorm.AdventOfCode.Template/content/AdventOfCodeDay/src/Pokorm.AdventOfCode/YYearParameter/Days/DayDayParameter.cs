namespace Pokorm.AdventOfCode.YYearParameter.Days;

// https://adventofcode.com/YearParameter/day/DayParameter
public class DayDayParameter(ILogger<DayDayParameter> logger)
{
#if (NoLines)
    public long Solve(string input)
#else
    public long Solve(string[] lines)
#endif
    {
#if (NoLines)
        var data = Parse(input);
#else
        var data = Parse(lines);
#endif

        var result = 0;

        return result;
    }

#if (NoLines)
    public long SolveBonus(string input)
#else
    public long SolveBonus(string[] lines)
#endif
    {
#if (NoLines)
        var data = Parse(input);
#else
        var data = Parse(lines);
#endif

        var result = 0;

        return result;
    }

#if (NoLines)
    static DayData Parse(string input)
#else
    static DayData Parse(string[] lines)
#endif
    {
        return new DayData();
    }

    record DayData()
    {

    }
}
