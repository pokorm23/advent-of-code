namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/19
public class Day19(ILogger<Day19> logger)
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        foreach (var dataDesign in data.Designs)
        {
            if (IsDesignPossible(dataDesign, data.Patters))
            {
                result++;
            }
        }

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
    }

    public bool IsDesignPossible(string text, HashSet<string> patterns)
    {
        if (text.Length == 0)
        {
            return true;
        }

        var possibles = patterns.Where(text.StartsWith)
                                .ToList();

        foreach (var possible in possibles)
        {
            var newText = text[possible.Length..];

            var next = IsDesignPossible(newText, patterns);

            if (next)
            {
                return true;
            }
        }

        return false;
    }

    private static DayData Parse(string[] lines)
    {
        var (patterns, designs) = lines.StrParts2();

        var p = patterns[0].FullSplit(',').ToHashSet();

        return new DayData(p, designs.ToList());
    }

    private record DayData(HashSet<string> Patters, List<string> Designs) { }
}
