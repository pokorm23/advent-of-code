namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/3
public class Day03
{
    private static DayData Parse(string input)
    {
        var directions = new List<Direction>();

        foreach (var c in input)
        {
            var d = c switch
            {
                '^'   => Direction.Top,
                '>'   => Direction.Right,
                '<'   => Direction.Left,
                'v'   => Direction.Bottom,
                var _ => throw new Exception()
            };

            directions.Add(d);
        }

        return new DayData(directions);
    }

    public long Solve(string input)
    {
        var data = Parse(input);

        var seen = new HashSet<Coord>();

        var pos = new Coord(0, 0);

        seen.Add(pos);

        foreach (var dataDirection in data.Directions)
        {
            pos = pos + GetVector(dataDirection);
            seen.Add(pos);
        }

        return seen.Count;
    }

    public long SolveBonus(string input)
    {
        var data = Parse(input);

        var seen = new HashSet<Coord>();

        var pos1 = new Coord(0, 0);
        var pos2 = new Coord(0, 0);

        seen.Add(pos1);
        seen.Add(pos2);

        foreach (var (i, dataDirection) in data.Directions.Index())
        {
            if (i % 2 == 0)
            {
                pos1 = pos1 + GetVector(dataDirection);
                seen.Add(pos1);
            }
            else
            {
                pos2 = pos2 + GetVector(dataDirection);
                seen.Add(pos2);
            }
        }

        return seen.Count;
    }

    private enum Direction
    {
        Top,
        Left,
        Right,
        Bottom
    }

    private record struct Vector(int X, int Y)
    {
        public static Vector Zero = new Vector(0, 0);

        public static Vector operator *(Vector c, int scale) => new Vector(c.X * scale, c.Y * scale);
    }

    private record struct Coord(int X, int Y)
    {
        public static Coord operator +(Coord c, Vector v) => new Coord(c.X + v.X, c.Y + v.Y);
    }

    private Vector GetVector(Direction d)
    {
        return d switch
        {
            Direction.Bottom => new (0, 1),
            Direction.Top    => new (0, -1),
            Direction.Right  => new (1, 0),
            Direction.Left   => new (-1, 0),
            var _            => throw new Exception()
        };
    }

    private record DayData(List<Direction> Directions) { }
}
