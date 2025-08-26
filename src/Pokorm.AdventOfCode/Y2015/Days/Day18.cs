namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/18
public class Day18(ILogger<Day18> logger)
{
    public long Solve(string[] lines) => Solve(lines, 100, false);

    public long SolveBonus(string[] lines) => Solve(lines, 100, true);

    public long Solve(string[] lines, int steps, bool lockCorners)
    {
        var data = Parse(lines, lockCorners);

        for (var s = 0; s < steps; s++)
        {
            var gridCursor = data.Data.Copy();

            foreach (var (c, (l, canChange)) in gridCursor.Values)
            {
                if (!canChange)
                {
                    continue;
                }

                var neib = gridCursor.GetValuedSiblings(c, Vector.All).Count(x => x.Value.Item1);

                var nextState = l ? neib is 2 or 3 : neib is 3;

                data.Data.Values[c] = (nextState, true);
            }
        }

        var result = data.Data.Values.Values.Count(x => x.Item1);

        return result;
    }

    private static DayData Parse(string[] lines, bool lockCorners)
    {
        var grid = Parser.ParseValuedGrid(lines, (c, coord) => (c == '#', true));

        if (lockCorners)
        {
            grid = grid.Transform((tuple, coord) => grid.GetSiblings(coord, Vector.All).Count() == 3 ? (true, false) : (tuple.Item1, true));
        }

        return new DayData(grid);
    }

    private record DayData(Grid<(bool, bool)> Data) { }
}
