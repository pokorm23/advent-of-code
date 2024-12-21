using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/19
public class Day19(ILogger<Day19> logger)
{
    public int Solve(string[] lines)
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

        long result = 0;

        var cache = new Dictionary<string, long>();

        foreach (var (i, dataDesign) in data.Designs.Index())
        {
            var patterns = data.Patters
                               .Where(x => dataDesign.Contains(x))
                               .ToList();

            var r = CountPossibleDesigns(dataDesign, patterns, cache);

            Debug.Assert(r >= 0);

            logger.LogInformation($"{i + 1}/{data.Designs.Count}: {r}");

            result += r;
        }

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


    public long CountPossibleDesigns(string text, IReadOnlyCollection<string> patterns, Dictionary<string, long> cache)
    {
        if (cache.TryGetValue(text, out var cachedCount))
        {
            return cachedCount;
        }

        if (text.Length == 0)
        {
            return 1;
        }

        long posCount = 0;

        foreach (var possible in patterns)
        {
            if (!text.StartsWith(possible))
            {
                continue;
            }

            var newText = text[possible.Length..];

            var next = CountPossibleDesigns(newText, patterns, cache);

            posCount += next;
        }

        cache.Add(text, posCount);

        return posCount;
    }

    private static DayData Parse(string[] lines)
    {
        var (patterns, designs) = lines.StrParts2();

        var p = patterns[0].FullSplit(',').ToHashSet();

        return new DayData(p, designs.ToList());
    }

    private record DayData(HashSet<string> Patters, List<string> Designs) { }
}
