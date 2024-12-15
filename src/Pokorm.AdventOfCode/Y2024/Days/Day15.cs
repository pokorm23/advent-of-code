using System.Diagnostics;
using System.Text;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/15
public class Day15(ILogger<Day15> logger)
{
    public long Solve(string[] lines) => Solve(lines, false);

    public long SolveBonus(string[] lines) => Solve(lines, true);

    private long Solve(string[] lines, bool widen)
    {
        var data = Parse(lines, widen);

        var dirStack = new Stack<Direction>(data.Directions.AsEnumerable().Reverse());

        var g = data.GridData;

        logger.LogDebug("Initial state:");

        foreach (var line in g.Grid.GetLines())
        {
            logger.LogDebug(line);
        }

        logger.LogDebug(" ");

        while (dirStack.TryPop(out var nextDir))
        {
            g = Run(nextDir, g);

            logger.LogDebug($"Move {nextDir.ToChar()}:");

            foreach (var line in g.Grid.GetLines())
            {
                logger.LogDebug(line);
            }

            logger.LogDebug(" ");
        }

        return g.Grid.Values.Where(x => x.Value is PositionType.Box)
                .Select(x => x.Key)
                .Sum(x => 100 * (x.Y + 1) + x.X + 1);
    }

    private GridData Run(Direction dir, GridData gridData)
    {
        var (grid, robotPos) = gridData.Copy();

        var v = dir.GetVector();
        var inDir = grid.TryGetCoordInDirection(robotPos, v);

        if (inDir is null)
        {
            return gridData;
        }

        var posInDir = grid.Values[inDir.Value];

        if (posInDir is PositionType.Wall)
        {
            return gridData;
        }

        Debug.Assert(posInDir is PositionType.Box or PositionType.BigBoxLeft or PositionType.BigBoxRight or PositionType.Free);

        if (posInDir is PositionType.Free)
        {
            grid.Values[robotPos] = PositionType.Free;
            grid.Values[inDir.Value] = PositionType.Robot;

            return new GridData(grid, inDir.Value);
        }

        if (posInDir is PositionType.Box)
        {
            return SolveSimpleBox(gridData, inDir.Value, v);
        }

        return SolveComplexBox(gridData, inDir.Value, v, posInDir == PositionType.BigBoxLeft);
    }

    private static GridData SolveSimpleBox(GridData gridData, Coord inDir, Vector v)
    {
        var (grid, robotPos) = gridData;

        Coord? c = inDir;

        var boxes = new List<Coord>()
        {
            inDir
        };

        var foundFree = false;

        while (true)
        {
            c = grid.TryGetCoordInDirection(c.Value, v);

            if (c is null)
            {
                break;
            }

            var posInC = grid.Values[c.Value];

            if (posInC is PositionType.Wall)
            {
                break;
            }

            if (posInC is PositionType.Free)
            {
                foundFree = true;

                break;
            }

            Debug.Assert(posInC is PositionType.Box);

            boxes.Add(c.Value);
        }

        if (!foundFree)
        {
            return gridData;
        }

        // move robot
        grid.Values[robotPos] = PositionType.Free;
        grid.Values[inDir] = PositionType.Robot;

        foreach (var coord in boxes)
        {
            var nc = coord + v;

            grid.Values[nc] = PositionType.Box;
        }

        return new GridData(grid, inDir);
    }

    private static GridData SolveComplexBox(GridData gridData, Coord inDir, Vector v, bool isLeftBox) => throw new NotImplementedException();

    private static DayData Parse(string[] lines, bool widen)
    {
        var splitIndex = lines.Index().FirstOrDefault((x) => string.IsNullOrWhiteSpace(x.Item)).Index;

        var (gridLines, dirs) = (lines[..splitIndex], lines[splitIndex..]);

        if (widen)
        {
            for (var i = 0; i < gridLines.Length; i++)
            {
                var line = gridLines[i];
                var newLine = new StringBuilder();

                foreach (var c in line)
                {
                    newLine.Append(c switch
                    {
                        '#'   => "##",
                        'O'   => "[]",
                        '.'   => "..",
                        '@'   => "@.",
                        var _ => throw new ArgumentOutOfRangeException(c.ToString())
                    });
                }

                gridLines[i] = newLine.ToString();
            }
        }

        var grid = Parser.ParseValuedGrid(gridLines, c =>
        {
            var type = c switch
            {
                '#'   => PositionType.Wall,
                '.'   => PositionType.Free,
                '@'   => PositionType.Robot,
                'O'   => PositionType.Box,
                '['   => PositionType.BigBoxLeft,
                ']'   => PositionType.BigBoxRight,
                var _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
            };

            return type;
        });

        grid.ValueCharFactory = c => c switch
        {
            PositionType.Wall        => '#',
            PositionType.Free        => '.',
            PositionType.Robot       => '@',
            PositionType.Box         => 'O',
            PositionType.BigBoxLeft  => '[',
            PositionType.BigBoxRight => ']',
            var _                    => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };

        if (!widen)
        {
            grid = grid.GetSubGrid(new Coord(1, 1), grid.Width - 1, grid.Height - 1);
        }
        else
        {
            grid = grid.GetSubGrid(new Coord(2, 1), grid.Width - 2, grid.Height - 1);
        }

        Debug.Assert(grid.Values.Count(x => x.Value is PositionType.Robot) == 1);

        var robotPos = grid.Values.Single(x => x.Value is PositionType.Robot);

        var gridData = new GridData(grid, robotPos.Key);

        var dirData = new List<Direction>();

        foreach (var d in string.Join("", dirs).ToLower())
        {
            dirData.Add(d.ToDirection());
        }

        return new DayData(gridData, dirData);
    }

    private enum PositionType
    {
        Free,
        Wall,
        Box,
        Robot,
        BigBoxLeft,
        BigBoxRight
    }

    private record GridData(Grid<PositionType> Grid, Coord RobotPosition)
    {
        public GridData Copy() => this with
        {
            Grid = this.Grid.Copy()
        };
    }

    private record DayData(GridData GridData, List<Direction> Directions) { }
}
