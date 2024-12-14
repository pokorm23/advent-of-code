namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/14
public partial class Day14(ILogger<Day14> logger)
{
    [GeneratedRegex(@"p=(?<px>-{0,1}\d+),(?<py>-{0,1}\d+) v=(?<vx>-{0,1}\d+),(?<vy>-{0,1}\d+)")]
    public static partial Regex RobotRegex { get; }

    public long SolveInGrid(string[] lines, Grid grid, int iterations)
    {
        var data = Parse(lines);

        var state = data.Robots.ToDictionary(x => x, x => x.InitialCoord);

        for (var i = 0; i < iterations; i++)
        {
            foreach (var r in data.Robots)
            {
                var c = state[r];

                var nc = r.GetNextPosition(c, grid);

                state[r] = nc;
            }
        }

        var middle = new Coord(grid.Width / 2, grid.Height / 2);

        var rq1 = state.Count(x => x.Value.X < middle.X && x.Value.Y < middle.Y);
        var rq2 = state.Count(x => x.Value.X > middle.X && x.Value.Y < middle.Y);
        var rq3 = state.Count(x => x.Value.X < middle.X && x.Value.Y > middle.Y);
        var rq4 = state.Count(x => x.Value.X > middle.X && x.Value.Y > middle.Y);

        return rq1 * rq2 * rq3 * rq4;
    }

    public long Solve(string[] lines) => SolveInGrid(lines, new Grid(101, 103), 100);

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
    }

    private static DayData Parse(string[] lines)
    {
        var robots = new List<Robot>();

        foreach (var line in lines)
        {
            var m = RobotRegex.Match(line);
            var px = int.Parse(m.Groups["px"].Value);
            var py = int.Parse(m.Groups["py"].Value);
            var vx = int.Parse(m.Groups["vx"].Value);
            var vy = int.Parse(m.Groups["vy"].Value);

            robots.Add(new Robot(new Coord(px, py), new Vector(vx, vy)));
        }

        return new DayData(robots);
    }

    private record Robot(Coord InitialCoord, Vector Velocity)
    {
        public Coord GetNextPosition(Coord currentPos, Grid grid)
        {
            var next1 = currentPos + this.Velocity;

            if (grid.IsIn(next1))
            {
                return next1;
            }

            var x = next1.X >= 0 && next1.X < grid.Width
                        ? next1.X
                        : next1.X < 0
                            ? next1.X + grid.Width
                            : next1.X - grid.Width;

            var y = next1.Y >= 0 && next1.Y < grid.Height
                        ? next1.Y
                        : next1.Y < 0
                            ? next1.Y + grid.Height
                            : next1.Y - grid.Height;

            var next2 = new Coord(x, y);

            return next2;
        }
    }

    private record DayData(List<Robot> Robots) { }
}
