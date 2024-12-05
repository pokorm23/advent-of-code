namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/5
public class Day05
{
    static DayData Parse(string[] lines)
    {
        var rules = new HashSet<Rule>();
        var data = new List<IReadOnlyCollection<int>>();
        var dataRead = false;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                dataRead = true;
                continue;
            }

            if (!dataRead)
            {
                var numbers = line.FullSplit('|');

                rules.Add(new Rule(int.Parse(numbers[0]), int.Parse(numbers[1])));
            }
            else
            {
                var numbers = line.FullSplit(',');

                data.Add(numbers.Select(int.Parse).ToList());
            }
        }

        return new DayData(rules, data);
    }

    public long Solve(string[] lines)
    {
        var data = Parse(lines);
        var result = 0;

        var headLookup = data.Rules.ToLookup(x => x.Head, x => x);

        var tailLookup =data.Rules.ToLookup(x => x.Tail, x => x);

        foreach (var line in data.Lines)
        {
            var seenTails = new HashSet<Rule>();
            var correct = true;
            
            foreach (var num in line)
            {
                var heads = headLookup[num];

                if (heads.Any(h => seenTails.Contains(h)))
                {
                    correct = false;
                    break;
                }

                var tails = tailLookup[num];

                foreach (var tail in tails)
                {
                    seenTails.Add(tail);
                }
            }

            if (correct)
            {
                var middle = line.ElementAt((int) (line.Count / 2));

                result += middle;
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

    private record Rule(int Head, int Tail);

    private record DayData(HashSet<Rule> Rules, IReadOnlyCollection<IReadOnlyCollection<int>> Lines)
    {
    }
}
