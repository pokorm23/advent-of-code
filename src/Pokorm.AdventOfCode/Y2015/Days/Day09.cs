using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/9
public class Day09
{
    // AlphaCentauri to Arbre = 108
    private static Map Parse(string[] lines)
    {
        var edges = new Dictionary<HashSet<string>, int>();

        foreach (var line in lines)
        {
            var parts = line.FullSplit('=');

            var cities = parts[0].FullSplit("to").ToHashSet();

            Debug.Assert(cities.Count == 2);

            var distance = int.Parse(parts[1]);

            edges.Add(cities, distance);
        }

        return new Map(edges);
    }

    public long Solve(string[] lines)
    {
        var data = Parse(lines);
        var result = 0;

        var vs = data.GetVertexes();

        foreach (var v in vs)
        {
            var visited = new HashSet<string>()
            {
                v
            };

            var subResult = Recurse(v, 0).Min();

            result = result == 0 ? subResult : Math.Min(subResult, result);

            continue;

            List<int> Recurse(string cur, int l)
            {
                visited.Add(cur);

                var lenghts = new List<int>();

                foreach (var (sv, sd) in data.GetSiblings(cur))
                {
                    if (visited.Contains(sv))
                    {
                        continue;
                    }

                    var sub = Recurse(sv, l + sd);

                    lenghts.Add(sub.Min());
                }

                if (lenghts.Count == 0)
                {
                    lenghts.Add(l);
                }

                return lenghts;
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

    private record Map(Dictionary<HashSet<string>, int> Edges)
    {
        public HashSet<string> GetVertexes()
        {
            return this.Edges.Keys.SelectMany(x => x).ToHashSet();
        }

        public Dictionary<string, int> GetSiblings(string v)
        {
            return this.Edges.Where(x => x.Key.Contains(v))
                       .Select(x => (x.Key.Single(y => y != v), x.Value))
                       .ToDictionary();
        }
    }
}
