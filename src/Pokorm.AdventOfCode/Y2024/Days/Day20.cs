using System.ComponentModel;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/20
public class Day20(ILogger<Day20> logger)
{
    public Dictionary<long, int> SolveCheats(string[] lines, int maxCheatLength)
    {
        var data = Parse(lines);

        var shortcuts = FindPathWithShorcuts(data, maxCheatLength);

        return shortcuts;
    }

    public long Solve(string[] lines) => SolveCheats(lines, 2).Where(x => x.Key >= 100).Sum(x => x.Value);

    public long SolveBonus(string[] lines) => SolveCheats(lines, 20).Where(x => x.Key >= 100).Sum(x => x.Value);

    private Dictionary<long, int> FindPathWithShorcuts(DayData data, int maxCheatLength)
    {
        List<Coord> seen = [ ];
        HashSet<(Coord S, Coord E)> seenCheats = [ ];

        // saved [ps] -> count
        var possibleShortcuts = new Dictionary<long, int>();

        var c = data.Start;

        while (true)
        {
            var i = seen.Count;

            var pastCoords = seen.ToList();

            // for each past coord in the path - construct possible cheats
            foreach (var (pcIndex, pc) in pastCoords.Index().Reverse())
            {
                var diff = pc - c;
                var cost = Math.Abs(diff.X) + Math.Abs(diff.Y);

                // 1. can make a cheat
                if (cost > maxCheatLength)
                {
                    continue;
                }

                /*// 2. in direction from start to end is a wall
                var anyWallOnStart = GetDirectionalUnitVectors(diff)
                    .Any(x => data.Grid.TryGetValuedCoordInDirection(pc, x).Value.Position is PositionType.Wall);

                if (!anyWallOnStart)
                {
                    continue;
                }

                // 3. in direction from end to start is a wall
                var anyWallOnEnd = GetDirectionalUnitVectors(-diff)
                    .Any(x => data.Grid.TryGetValuedCoordInDirection(c, x).Value.Position is PositionType.Wall);

                if (!anyWallOnEnd)
                {
                    continue;
                }*/

                // 4. already seen cheat
                if (!seenCheats.Add((c, pc)))
                {
                    continue;
                }

                var newWay = i - pcIndex - cost;

                if (newWay > 0)
                {
                    possibleShortcuts.AddOrUpdate(newWay, 1, v => v + 1);
                }
            }

            if (c == data.End)
            {
                seen.Add(c);

                break;
            }

            var nPos = data.Grid.GetValuedSiblings(c, Vector.Directional)
                           .Where(x => !seen.Contains(x.Coord) && x.Value.Position is not PositionType.Wall)
                           .ToList();

            if (nPos.Count == 0)
            {
                throw new Exception("Cannot find path to the end");
            }

            if (nPos.Count != 1)
            {
                throw new Exception("Multiple possible paths found");
            }

            var n = nPos.Single();

            seen.Add(c);

            c = n.Coord;
        }

        return possibleShortcuts;

        Vector[] GetDirectionalUnitVectors(Vector source)
        {
            var xDir = source.X != 0 ? new Vector(Math.Sign(source.X), 0) : Vector.Zero;
            var yDir = source.Y != 0 ? new Vector(0, Math.Sign(source.Y)) : Vector.Zero;

            return [ xDir, yDir ];
        }
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
