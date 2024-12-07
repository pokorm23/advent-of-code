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
            if (Recurse(e.Test, e.Quefs))
            {
                result += e.Test;
            }
        }

        static bool Recurse(long test, List<long> coefs)
        {
            var plusCase = coefs[0] + coefs[1];
            var timeCase = coefs[0] * coefs[1];

            if (coefs.Count == 2)
            {
                return plusCase == test || timeCase == test;
            }

            var result1 = Recurse(test, [ plusCase, ..coefs[2..] ]);

            if (result1)
            {
                return result1;
            }

            return Recurse(test, [ timeCase, ..coefs[2..] ]);
        }

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
    }

    private record Eq(long Test, List<long> Quefs);

    private record DayData(List<Eq> Equations) { }
}
