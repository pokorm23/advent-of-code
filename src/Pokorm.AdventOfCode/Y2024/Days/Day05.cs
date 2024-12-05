using System.Collections.Immutable;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/5
public class Day05
{
    private static DayData Parse(string[] lines)
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

        var tailLookup = data.Rules.ToLookup(x => x.Tail, x => x);

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

        var headLookup = data.Rules.ToLookup(x => x.Head, x => x);

        var tailLookup = data.Rules.ToLookup(x => x.Tail, x => x);

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

            /*Console.WriteLine($"{string.Join(",", line)}");
            Console.WriteLine($" - applied: {string.Join("; ", ruleApplied)}");
            var touched = data.Rules.Where(x => line.Contains(x.Head) && line.Contains(x.Tail) ).ToHashSet();
            Console.WriteLine($" - all touched: {string.Join("; ", touched)}");
            Console.WriteLine($" - nums in rules: {string.Join("; ", touched.SelectMany(x => new int[] {x.Tail, x.Head}).ToHashSet())}");
            Console.WriteLine($" - ordered: {string.Join("; ", touched.SelectMany(x => new int[] {x.Tail, x.Head}).ToHashSet().Order(data.CreateComparer()))}");
            Console.WriteLine($" - ordered: {string.Join("; ", data.GetOrderedHeads().Where(x => line.Contains(x)))}");
            Console.WriteLine();*/

            var ordered = line.ToImmutableSortedSet(data.CreateComparer());

            var co = ordered.SequenceEqual(line);

            if (co != correct)
            {
                Console.WriteLine($"{string.Join(",", line)}: FAIL");
                continue;
            }

            if (!correct)
            {
                Console.WriteLine($"{string.Join(",", line)}: OK");
                continue;
            }

            Console.WriteLine($"{string.Join(",", line)}: {string.Join(",", ordered)}");

            var middle = ordered.ElementAt((int) (line.Count / 2));

            result += middle;
        }

        return result;
    }

    private record NumberInLine(int Index, int Value)
    {
        public override string ToString() => $"{this.Value} [{this.Index}]";
    }

    private record RulePos(Rule Rule, NumberInLine Num)
    {
        public override string ToString() => $"{this.Rule} - {this.Num}";
    }


    private record Rule(int Head, int Tail)
    {
        public override string ToString() => $"{this.Head}|{this.Tail}";
    }

    private record DayData(HashSet<Rule> Rules, IReadOnlyCollection<IReadOnlyCollection<int>> Lines)
    {
        public IComparer<int> CreateComparer()
        {
            var ordered = GetOrderedHeads();

            return Comparer<int>.Create((x, y) =>
            {
                var xPos = ordered.IndexOf(x);
                var yPos = ordered.IndexOf(y);

                if (xPos == -1 || yPos == -1)
                {
                    return 0;
                }

                return xPos.CompareTo(yPos);
            });
        }

        public List<int> GetOrderedHeads()
        {
            var lookup = Rules
                         .GroupBy(x => x.Head, x => Rules.Where(y => y.Head == x.Head).Select(y => y.Tail))
                .ToDictionary(x => x.Key, x => x.SelectMany(y => y).ToHashSet());

            foreach (var rule in this.Rules)
            {
                if (!lookup.TryGetValue(rule.Tail, out _))
                {
                    lookup.Add(rule.Tail, []);
                }
            }

            var uniqueNumbers = lookup.Keys.ToArray();

            var result = new List<int>();

            for (var i = 0; i < uniqueNumbers.Length; i++)
            {
                var num = uniqueNumbers[i]; // 75

                if (result.Count == 0)
                {
                    result.Add(num);
                    continue;
                }

                var wasInserted = false;

                for (int j = 0; j < result.Count; j++)
                {
                    var numToCompare = result[j];

                    if (lookup[num].Contains(numToCompare))
                    {
                        result.Insert(j, num);
                        wasInserted = true;
                        break;
                    }
                }

                if (!wasInserted)
                {
                    result.Add(num);
                }
            }

            return result;
        }
    }
}
