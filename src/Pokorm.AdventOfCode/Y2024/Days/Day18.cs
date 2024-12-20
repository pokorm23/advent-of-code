using System.Collections.Concurrent;
using System.ComponentModel;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/18
public class Day18(ILogger<Day18> logger)
{
    public int SolvePartOne(string[] lines, int fallenBytes, int size)
    {
        var data = Parse(lines);

        var graph = CreateGrid(data, fallenBytes, size);

        return FindShortestPath(graph, Coord.Zero, new Coord(size - 1, size - 1)) ?? throw new Exception("no path");
    }

    public int Solve(string[] lines) => SolvePartOne(lines, 1024, 71);

    public int SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
    }

    public int? FindShortestPath(Grid<PositionType> graph, Coord source, Coord targetCoord)
    {
        logger.LogInformation(graph.ToString());

        var q = new PriorityQueue<Coord, int>();

        var dist = new ConcurrentDictionary<Coord, int>();
        var prev = new ConcurrentDictionary<Coord, HashSet<Coord>>();

        dist.TryAdd(source, 0);

        q.Enqueue(source, 0);

        while (q.Count > 0)
        {
            var u = q.Dequeue();

            if (u == targetCoord)
            {
                return dist[u];
            }

            foreach (var v in graph.GetSiblings(u, Vector.Directional))
            {
                if (graph.Values[v] is PositionType.Corrupted)
                {
                    continue;
                }

                var alt = dist[u] + 1;

                if (dist.TryGetValue(v, out var distV) && alt >= distV)
                {
                    continue;
                }

                prev.AddOrUpdate(v, _ => [ u ], (_, c) => [ u ]);
                dist.AddOrUpdate(v, _ => alt, (_, c) => alt);
                q.Enqueue(v, alt);
            }
        }

        return null;
    }

    public Grid<PositionType> CreateGrid(DayData data, int fallenBytes, int size)
    {
        if (fallenBytes > data.IncomingBytePositions.Count)
        {
            throw new Exception();
        }

        var stream = data.IncomingBytePositions.Take(fallenBytes).ToHashSet();

        var values = new Dictionary<Coord, PositionType>();

        var e = Enumerable.Range(0, size).ToArray();

        foreach (var c in e.SelectMany(x => e.Select(y => new Coord(x, y))))
        {
            values.Add(c, stream.Contains(c) ? PositionType.Corrupted : PositionType.Free);
        }

        return new Grid<PositionType>(values, size, size);
    }

    public static DayData Parse(string[] lines)
    {
        var result = new List<Coord>();

        foreach (var line in lines)
        {
            var nums = line.FullSplit(',').Nums2();

            result.Add(new Coord(nums));
        }

        return new DayData(result);
    }

    public enum PositionType
    {
        [Description(".")]
        Free,

        [Description("#")]
        Corrupted
    }

    public record DayData(List<Coord> IncomingBytePositions) { }
}
