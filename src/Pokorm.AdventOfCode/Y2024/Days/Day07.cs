namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/7
public class Day07
{
    private static DayData Parse(string[] lines)
    {
        var eqs = new List<Eq>();

        foreach (var line in lines)
        {
            var s = line.FullSplit(':');
            var other = s[1].FullSplit(' ').Select(long.Parse);

            eqs.Add(new Eq(long.Parse(s[0]), other.ToList()));
        }

        return new DayData(eqs);
    }

    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var result = 0L;

        foreach (var e in data.Equations)
        {
            if (MatchesTest(e.Test, e.Quefs, false))
            {
                result += e.Test;
            }
        }

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0L;

        foreach (var e in data.Equations)
        {
            if (MatchesTest(e.Test, e.Quefs, true))
            {
                result += e.Test;
            }
        }

        return result;
    }

    private static bool MatchesTest(long test, List<long> coefs, bool useConcatenation)
    {
        var plusCase = coefs[0] + coefs[1];
        var timeCase = coefs[0] * coefs[1];
        var concatCase = !useConcatenation ? (long?) null : long.Parse($"{coefs[0]}{coefs[1]}");

        if (coefs.Count == 2)
        {
            return plusCase == test || timeCase == test || concatCase == test;
        }

        var result1 = MatchesTest(test, [ plusCase, ..coefs[2..] ], useConcatenation);

        if (result1)
        {
            return result1;
        }

        var result2 = MatchesTest(test, [ timeCase, ..coefs[2..] ], useConcatenation);

        if (result2 || concatCase is null)
        {
            return result2;
        }

        var result3 = MatchesTest(test, [ concatCase.Value, ..coefs[2..] ], useConcatenation);

        return result3;
    }

    private record Eq(long Test, List<long> Quefs);

    private record DayData(List<Eq> Equations) { }
}
