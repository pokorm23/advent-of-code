using System.Collections.Immutable;
using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/5
public class Day05
{
    public static DayData Parse(string[] lines)
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
            var ruleApplied = new HashSet<RulePos>();

            foreach (var nn in line.Select((x, i) => new NumberInLine(i, x)))
            {
                var (_, num) = nn;

                var heads = headLookup[num].ToList();

                var matchedRules = heads.Where(h => seenTails.Contains(h)).ToList();

                if (matchedRules.Count > 0)
                {
                    foreach (var r in matchedRules)
                    {
                        ruleApplied.Add(new RulePos(r, nn));
                    }

                    correct = false;

                    //break;
                }

                var tails = tailLookup[num];

                foreach (var tail in tails)
                {
                    seenTails.Add(tail);
                }
            }

            if (correct)
            {
                //Console.WriteLine($"{string.Join(",", line)}: OK");

                continue;
            }

            var context = ruleApplied.Select(x => x.Rule).ToHashSet();

            context.UnionWith(data.Rules.Where(x => line.Contains(x.Head)));

            var comparer = data.CreateComparer(context);

            foreach (var rule in context)
            {
                if (comparer.Compare(rule.Head, rule.Tail) != -1)
                {
                    throw new Exception($"Comparer not consistent");
                }
            }

            var ordered = line.ToImmutableSortedSet(comparer);

            var co = ordered.SequenceEqual(line);

            if (co != correct)
            {
                throw new Exception($"Comparing not consistent");
            }

            //Console.WriteLine($"{string.Join(",", line)}: {string.Join(",", ordered)}");

            Debug.Assert(ordered.Count == line.Count);
            Debug.Assert(ordered.Count % 2 == 1);

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


    public record Rule(int Head, int Tail)
    {
        public override string ToString() => $"{this.Head}|{this.Tail}";
    }

    public record DayData(HashSet<Rule> Rules, IReadOnlyCollection<IReadOnlyCollection<int>> Lines)
    {
        public IComparer<int> CreateComparer(ISet<Rule> context)
        {
            var ordered = GetOrderedHeads(context);

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

        public List<int> GetOrderedHeads(ISet<Rule> context)
        {
            var lookup = this.Rules.Where(x => context.Contains(x))
                             .GroupBy(x => x.Head, x => x.Tail)
                             .ToDictionary(x => x.Key, x => x.ToHashSet());

            foreach (var rule in this.Rules)
            {
                if (!lookup.TryGetValue(rule.Tail, out var _))
                {
                    lookup.Add(rule.Tail, [ ]);
                }
            }

            var expandCache = new Dictionary<int, HashSet<int>>();
            var conflict = new HashSet<int>();

            HashSet<int> Expand(int n)
            {
                if (expandCache.TryGetValue(n, out var r))
                {
                    return r;
                }

                var newSet = new HashSet<int>();

                newSet.Add(n);

                foreach (var i in lookup[n].ToList())
                {
                    if (!conflict.Add(i))
                    {
                        throw new Exception();
                    }

                    newSet.UnionWith(Expand(i));
                    conflict.Remove(i);
                }

                expandCache.Add(n, newSet);

                return newSet;
            }

            // expand
            foreach (var (key, value) in lookup)
            {
                value.UnionWith(Expand(key));
            }

            var uniqueNumbers = lookup.Keys.ToArray();

            var result = new List<int>();

            for (var i = 0; i < uniqueNumbers.Length; i++)
            {
                var num = uniqueNumbers[i];

                if (result.Count == 0)
                {
                    result.Add(num);

                    continue;
                }

                var wasInserted = false;

                for (var j = 0; j < result.Count; j++)
                {
                    var numToCompare = result[j];

                    if (!lookup[num].Contains(numToCompare))
                    {
                        continue;
                    }

                    result.Insert(j, num);
                    wasInserted = true;

                    break;
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
