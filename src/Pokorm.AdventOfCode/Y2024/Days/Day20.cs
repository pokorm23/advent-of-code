using System.ComponentModel;
using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/20
public class Day20(ILogger<Day20> logger)
{
    public Dictionary<int, int> SolveCheats(string[] lines)
    {
        var data = Parse(lines);

        var counter = new Dictionary<int, int>();

        var coords = FindPath(data,[])!;

        foreach (var (i, coord) in coords.Index())
        {
            var modifications = data.Grid.GetValuedSiblings(coord, Vector.Directional)
                                    .Where(x => x.Value is PositionType.Wall)
                                    .Select(x => x.Coord)
                                    .ToList();

            foreach (var m in modifications)
            {
                var modifications2 = data.Grid.GetValuedSiblings(m, Vector.Directional)
                                         .Where(x => x.Coord != coord)
                                         .Select(x => x.Coord)
                                         .ToList();

                foreach (var m2 in modifications2)
                {
                    var alteredGrid = data.Grid.Transform((t, c) => c == m || c == m2 ? PositionType.Free : t);

                    logger.LogDebug(alteredGrid.ToString());

                    var altSeen = FindPath(data with
                    {
                        End = coord
                    }, [])!;

                    altSeen.Add(data.Start);
                    altSeen.Add(m);

                    if (coords.Contains(m2)) // is on path
                    {
                        // ok i actually need dij's
                        var ppp = coords[..(coords.IndexOf(m2))];
                        altSeen.AddRange(ppp);
                    }

                    var newCoords = FindPath(data with
                    {
                        Start = m2,
                        Grid = alteredGrid
                    }, altSeen);

                    if (newCoords is null)
                    {
                        continue;
                    }

                    var saved = Math.Max(0, coords.Count - (altSeen.Count + newCoords.Count + 2));

                    if (saved == 0)
                    {
                        continue;
                    }

                    if (!counter.TryAdd(saved, 1))
                    {
                        counter[saved]++;
                    }
                }
            }
        }

        return counter;
    }

    public long Solve(string[] lines) => SolveCheats(lines).Where(x => x.Key >= 100).Sum(x => x.Value);

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
    }

    private List<Coord>? FindPath(DayData data, List<Coord> seen)
    {
        seen = seen.ToList();
        var c = data.Start;

        while (c != data.End)
        {
            var next = data.Grid.GetValuedSiblings(c, Vector.Directional)
                           .ToList();

            var nPos = next.Where(x => !seen.Contains(x.Coord) && x.Value is PositionType.Free).ToList();

            if (nPos.Count == 0)
            {
                return null;
            }
            
            if (nPos.Count != 1)
            {
                Debugger.Break();
            }

            var n = nPos.Single();

            seen.Add(c);

            c = n.Coord;
        }

        return seen;
    }

    private static DayData Parse(string[] lines)
    {
        Coord? start = null;
        Coord? end = null;

        var g = Parser.ParseValuedGrid(lines, (c, coord) =>
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

            return c == '.' ? PositionType.Free : PositionType.Wall;
        });

        return new DayData(start!.Value, end!.Value, g);
    }

    private enum PositionType
    {
        [Description(".")]
        Free,

        [Description("#")]
        Wall
    }

    private record DayData(Coord Start, Coord End, Grid<PositionType> Grid) { }
}
