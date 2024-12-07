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
        //Console.WriteLine();
        var data = Parse(lines);
        var result = 0;

        var vs = data.GetVertexes();

        foreach (var v in vs)
        {
            var (subResult, stack) = Recurse(v, 0, [ ], [ ]);

            //Console.WriteLine($"SubResult: {subResult} in {string.Join(" -> ", stack)}");

            result = result == 0 ? subResult : Math.Min(subResult, result);

            continue;

            (int Result, List<string> Stack) Recurse(string cur, int l, HashSet<string> visited, Stack<string> stack)
            {
                visited.Add(cur);
                stack.Push(cur);

                var indent = new string(' ', (stack.Count - 1) * 2);

                //Console.WriteLine($"{indent}[{cur}] start");

                var isEnd = visited.Count == vs.Count;

                if (isEnd)
                {
                    //Console.WriteLine($"{indent}[{cur}] end as {l}");
                    var r = (l, stack.AsEnumerable().Reverse().ToList());

                    visited.Remove(cur);
                    stack.Pop();

                    return r;
                }

                var lenghts = new List<(int Result, List<string> Stack)>();

                var siblings = data.GetSiblings(cur)
                                   .Where(c => !visited.Contains(c.Key))
                                   .ToList();

                //Console.WriteLine($"{indent}[{cur}] siblings: {string.Join(" ; ", siblings.Select(x => x.Key))}");

                foreach (var (sv, sd) in siblings)
                {
                    //Console.WriteLine($"{indent}[{cur}] go to {sv}");

                    var sub = Recurse(sv, l + sd, visited, stack);

                    //Console.WriteLine($"{indent}[{cur}] finished {sv} as {sub.Result}");

                    lenghts.Add(sub);
                }

                visited.Remove(cur);
                stack.Pop();

                foreach (var (i, list) in lenghts)
                {
                    //Console.WriteLine($"{indent}[{cur}] potential: {i} in {string.Join(" -> ", list)}");
                }

                return lenghts.MinBy(x => x.Result);
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
