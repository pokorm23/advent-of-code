using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/9
public class Day09
{
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
        //Console.WriteLine();
        var data = Parse(lines);

        var result = FindDistance(data);

        return result;
    }
    
    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = FindDistance(data, true);

        return result;
    }

    private static int FindDistance(Map data, bool longest = false)
    {
        var result = 0;
        var vs = data.GetVertexes();

        foreach (var v in vs)
        {
            var subResult = Recurse(v, 0, [ ], [ ]);

            result = result == 0 ? subResult : (longest ? Math.Max(subResult, result) : Math.Min(subResult, result));

            continue;

            int Recurse(string cur, int l, HashSet<string> visited, Stack<string> stack)
            {
                visited.Add(cur);
                stack.Push(cur);

                var isEnd = visited.Count == vs.Count;

                if (isEnd)
                {
                    visited.Remove(cur);
                    stack.Pop();

                    return l;
                }

                var lenghts = new List<int>();

                var siblings = data.GetSiblings(cur)
                                   .Where(c => !visited.Contains(c.Key))
                                   .ToList();

                foreach (var (sv, sd) in siblings)
                {
                    var sub = Recurse(sv, l + sd, visited, stack);

                    lenghts.Add(sub);
                }

                visited.Remove(cur);
                stack.Pop();

                return longest ?  lenghts.Max() : lenghts.Min();
            }
        }

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
