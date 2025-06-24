using System.ComponentModel;
using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/20
public class Day20(ILogger<Day20> logger)
{
    public Dictionary<int, int> SolveCheats(string[] lines)
    {
        var data = Parse(lines);

        var coords = FindPath(data, [ ])!;

        var shortcuts = FindShortcuts(data, coords);

        return shortcuts;
    }

    public long Solve(string[] lines) => SolveCheats(lines).Where(x => x.Key >= 100).Sum(x => x.Value);

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
    }

    private Dictionary<int, int> FindShortcuts(DayData data, List<Coord> origPath)
    {
        origPath = origPath.ToList();

        // saved [ps] -> count
        var possibleShortcuts = new Dictionary<int, int>();

        var seenCheats = new HashSet<Coord>();

        foreach (var (i, c) in origPath.Index())
        {
            var next1 = data.Grid.GetValuedSiblings(c, Vector.Directional)
                            .Where(x => x.Value.Position is PositionType.Wall)
                            .ToList();

            // just last
            if (c == data.End)
            {
                break;
            }

            foreach (var (c1, v1) in next1)
            {
                var next2 = data.Grid.GetValuedSiblings(c1, Vector.Directional)
                                .Select(x => (x.Coord, x.Value, Index: origPath.IndexOf(x.Coord)))
                                .Where(x => x.Value.Position is PositionType.Free && x.Index > i)
                                .ToList();

                // segmentation fault
                if (next2.Count == 0)
                {
                    continue;
                }

                var (c2, _, ic2) = next2.MaxBy(x => x.Index);

                if (seenCheats.Contains(c1))
                {
                    continue;
                }

                var cost = 1;

                var newWay = origPath.IndexOf(c2) - i - 1 - cost /* extra path */;

                /*var newGrid = data.Grid.Transform((t, cc) =>
                {
                    if (cc == c1)
                    {
                        return PositionType.One;
                    }

                    if (cc == c2)
                    {
                        return PositionType.Two;
                    }

                    if (cc == c)
                    {
                        return PositionType.F;
                    }

                    return t;
                });

                if (newWay == 2)
                {
                    logger.LogDebug(newGrid.ToString());
                }*/

                if (newWay > 0)
                {
                    possibleShortcuts.AddOrUpdate(newWay, 1, v => v + 1);
                }

                seenCheats.Add(c1);

                /*cost += v2 == PositionType.Wall ? 1 : 0;*/
            }
        }

        return possibleShortcuts;
    }

    private List<Coord>? FindPath(DayData data, List<Coord> seen)
    {
        seen = seen.ToList();
        var c = data.Start;

        while (c != data.End)
        {
            var next = data.Grid.GetValuedSiblings(c, Vector.Directional)
                           .ToList();

            var nPos = next.Where(x => !seen.Contains(x.Coord) && x.Value.Position is not PositionType.Wall).ToList();

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

        seen.Add(c);

        return seen;
    }

    private DayData Parse(string[] lines)
    {
        Coord? start = null;
        Coord? end = null;

        var g = Parser.ParseValuedGrid(lines, (c, coord) =>
        {
            if (c == 'S')
            {
                start = coord;

                return (PositionType.Free, 'S');
            }

            if (c == 'E')
            {
                end = coord;

                return (PositionType.Free, 'E');
            }

            return c == '.' ? (PositionType.Free, '.') : (PositionType.Wall, '#');
        });

        g.ValueCharFactory = tuple => tuple.Item2;

        //logger.LogDebug(g.ToString());

        return new DayData(start!.Value, end!.Value, g);
    }

    private enum PositionType
    {
        [Description(".")]
        Free,

        [Description("#")]
        Wall
    }

    private record DayData(Coord Start, Coord End, Grid<(PositionType Position, char Char)> Grid) { }
}
