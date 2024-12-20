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

        var (shortest, _) = FindPathsWithLeastPoints(data.Grid, start, data.End);

        Debug.Assert(shortest.HasValue);

        return shortest.Value;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var start = new DirectionCoord(data.Start, Vector.Right);

        var (_, paths) = FindPathsWithLeastPoints(data.Grid, start, data.End);

        var g = data.Grid.Transform((c, coord) =>
        {
            if (paths.Contains(coord))
            {
                return PositionType.Occupied;
            }

            return c;
        });

        logger.LogDebug(g.ToString());

        return paths.Count;
    }

    private static (long? MinimalScore, HashSet<Coord> AllPathCoords) FindPathsWithLeastPoints(Grid<PositionType> graph, DirectionCoord source, Coord targetCoord)
    {
        var q = new PriorityQueue<DirectionCoord, long>();

        var dist = new ConcurrentDictionary<DirectionCoord, long>();
        var prev = new ConcurrentDictionary<DirectionCoord, HashSet<DirectionCoord>>();

        dist.TryAdd(source, 0);

        q.Enqueue(source, 0);

        while (q.Count > 0)
        {
            var u = q.Dequeue();

            if (u.Coord == targetCoord)
            {
                var result = new HashSet<Coord>()
                {
                    source.Coord,
                    targetCoord
                };

                var cur = prev[u];

                while (cur.Count > 0)
                {
                    result.UnionWith(cur.Select(x => x.Coord));

                    cur = cur.Select(x => prev.TryGetValue(x, out var s) ? s : [ ]).SelectMany(x => x).ToHashSet();
                }

                return (dist[u], result);
            }

            var dir = new List<(DirectionCoord Coord, long Points)>();

            var directCoord = graph.TryGetValuedCoordInDirection(u.Coord, u.Direction);

            if (directCoord.Coord.HasValue && directCoord.Value is PositionType.Free)
            {
                dir.Add((new DirectionCoord(directCoord.Coord.Value, u.Direction)
                            {
                                PreviousCoord = u.Coord
                            }, 1));
            }

            foreach (var vector in Vector.Directional.Where(x => x != u.Direction))
            {
                var (coord, value) = graph.TryGetValuedCoordInDirection(u.Coord, vector);

                if (coord is null || value is PositionType.Wall)
                {
                    continue;
                }

                dir.Add((new DirectionCoord(u.Coord, vector)
                            {
                                PreviousCoord = u.Coord
                            }, 1000));
            }

            foreach (var (v, points) in dir)
            {
                var alt = dist[u] + points;

                if (dist.TryGetValue(v, out var distV) && alt > distV)
                {
                    continue;
                }

                prev.AddOrUpdate(v, _ => [ u ], (_, c) =>
                {
                    HashSet<DirectionCoord> newOnes = [ ..c.Where(x => x != v), u ];

                    var min = newOnes.Select(x => dist[x]).Min();

                    return newOnes.Select(x => (x, dist[x]))
                                  .Where(x => x.Item2 == min)
                                  .Select(x => x.x).ToHashSet();
                });

                dist.AddOrUpdate(v, _ => alt, (_, c) => alt);
                q.Enqueue(v, alt);
            }
        }

        return (null, [ ]);
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
            PositionType.Wall     => '#',
            PositionType.Free     => '.',
            PositionType.Occupied => 'O',
            var _                 => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };

        return new (grid, start.Value, end.Value);
    }

    private enum PositionType
    {
        Free,
        Wall,
        Occupied
    }

    private record DayData(Grid<PositionType> Grid, Coord Start, Coord End);
}
