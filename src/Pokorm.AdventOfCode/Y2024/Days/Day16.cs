using System.Collections.Concurrent;
using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/16
public class Day16(ILogger<Day16> logger)
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var start = new DirectionCoord(data.Start, Vector.Right);

        var shortest = FindPathWithLeastPolongs(data.Grid, start, data.End);

        Debug.Assert(shortest.HasValue);

        return shortest.Value;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
    }

    private static long? FindPathWithLeastPolongs(Grid<PositionType> graph, DirectionCoord source, Coord targetCoord)
    {
        var q = new PriorityQueue<DirectionCoord, long>();

        var dist = new ConcurrentDictionary<DirectionCoord, long>();
        var prev = new ConcurrentDictionary<DirectionCoord, DirectionCoord>();

        dist.TryAdd(source, 0);

        q.Enqueue(source, 0);

        while (q.Count > 0)
        {
            var u = q.Dequeue();

            if (u.Coord == targetCoord)
            {
                return dist[u];
            }

            var dir = new List<(DirectionCoord Coord, long Points)>();

            var directCoord = graph.TryGetValuedCoordInDirection(u.Coord, u.Direction);

            if (directCoord.Coord.HasValue && directCoord.Value is PositionType.Free)
            {
                dir.Add((new DirectionCoord(directCoord.Coord.Value, u.Direction), 1));
            }

            foreach (var vector in Vector.Directional.Where(x => x != u.Direction))
            {
                var (coord, value) = graph.TryGetValuedCoordInDirection(u.Coord, vector);

                if (coord is null || value is PositionType.Wall)
                {
                    continue;
                }

                dir.Add((new DirectionCoord(u.Coord, vector), 1000));
            }

            foreach (var (v, points) in dir)
            {
                var alt = dist[u] + points;

                if (!dist.TryGetValue(v, out var distV) || alt < distV)
                {
                    prev.AddOrUpdate(v, _ => u, (_, c) => u);
                    dist.AddOrUpdate(v, _ => alt, (_, c) => alt);
                    q.Enqueue(v, alt);
                }
            }
        }

        return null;
    }

    private static DayData Parse(string[] gridLines)
    {
        Coord? start = null;
        Coord? end = null;

        var grid = Parser.ParseValuedGrid(gridLines, (c, coord) =>
        {
            if (c == 'S')
            {
                start = coord;

                return PositionType.Free;
            }

            if (c == 'E')
            {
                end = coord;

                return PositionType.Free;
            }

            var type = c switch
            {
                '#'   => PositionType.Wall,
                '.'   => PositionType.Free,
                var _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
            };

            return type;
        });

        Debug.Assert(start is not null && end is not null);

        grid.ValueCharFactory = c => c switch
        {
            PositionType.Wall => '#',
            PositionType.Free => '.',
            var _             => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };

        return new (grid, start.Value, end.Value);
    }

    private enum PositionType
    {
        Free,
        Wall
    }

    private record DayData(Grid<PositionType> Grid, Coord Start, Coord End);
}
