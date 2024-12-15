using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/15
public class Day15(ILogger<Day15> logger)
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

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

    public long SolveBonus(string[] lines) => 0;

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

        Debug.Assert(posInDir is PositionType.Box or PositionType.Free);

        if (posInDir is PositionType.Free)
        {
            grid.Values[robotPos] = PositionType.Free;
            grid.Values[inDir.Value] = PositionType.Robot;

            return new GridData(grid, inDir.Value);
        }

        Coord? c = inDir.Value;

        var boxes = new List<Coord>()
        {
            inDir.Value
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
        grid.Values[inDir.Value] = PositionType.Robot;

        foreach (var coord in boxes)
        {
            var nc = coord + v;

            grid.Values[nc] = PositionType.Box;
        }

        return new GridData(grid, inDir.Value);
    }

    private static DayData Parse(string[] lines)
    {
        var splitIndex = lines.Index().FirstOrDefault((x) => string.IsNullOrWhiteSpace(x.Item)).Index;

        var (gridLines, dirs) = (lines[..splitIndex], lines[splitIndex..]);

        var grid = Parser.ParseValuedGrid(gridLines, c =>
        {
            var type = c switch
            {
                '#'   => PositionType.Wall,
                '.'   => PositionType.Free,
                '@'   => PositionType.Robot,
                'O'   => PositionType.Box,
                var _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
            };

            return type;
        });

        grid.ValueCharFactory = c => c switch
        {
            PositionType.Wall  => '#',
            PositionType.Free  => '.',
            PositionType.Robot => '@',
            PositionType.Box   => 'O',
            var _              => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };

        grid = grid.GetSubGrid(new Coord(1, 1), grid.Width - 1, grid.Height - 1);

        Debug.Assert(grid.Values.Count(x => x.Value is PositionType.Robot) == 1);

        var robotPos = grid.Values.Single(x => x.Value is PositionType.Robot);

        var gridData = new GridData(grid, robotPos.Key);

        var dirData = new List<Direction>();

        foreach (var d in string.Join("", dirs).ToLower())
        {
            dirData.Add(d switch
            {
                '<'   => Direction.Left,
                '>'   => Direction.Right,
                'v'   => Direction.Bottom,
                '^'   => Direction.Top,
                var _ => throw new ArgumentOutOfRangeException()
            });
        }

        return new DayData(gridData, dirData);
    }

    private enum PositionType
    {
        Free,
        Wall,
        Box,
        Robot
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
