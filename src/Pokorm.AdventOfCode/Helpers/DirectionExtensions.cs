namespace Pokorm.AdventOfCode.Helpers;

public static class DirectionExtensions
{
    public static Vector GetVector(this Direction dir)
    {
        return dir switch
        {
            Direction.Bottom => new Vector(0, 1),
            Direction.Top    => new Vector(0, -1),
            Direction.Left   => new Vector(-1, 0),
            Direction.Right  => new Vector(1, 0),
            var _            => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    public static Direction ToDirection(this char dir)
    {
        return char.ToLower(dir) switch
        {
            'v'   => Direction.Bottom,
            '^'   => Direction.Top,
            '<'   => Direction.Left,
            '>'   => Direction.Right,
            var _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    public static char ToChar(this Direction dir)
    {
        return dir switch
        {
            Direction.Bottom => 'v',
            Direction.Top    => '^',
            Direction.Left   => '<',
            Direction.Right  => '>',
            var _            => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    public static char ToMultiDirChar(this Direction dir)
    {
        return dir switch
        {
            Direction.Right                                                      => '\u2192',
            Direction.Left                                                       => '\u2190',
            Direction.Bottom                                                     => '\u2193',
            Direction.Top                                                        => '\u2191',
            { } v when v.HasFlag(Direction.Right) && v.HasFlag(Direction.Bottom) => '\u2198',
            { } v when v.HasFlag(Direction.Right) && v.HasFlag(Direction.Top)    => '\u2197',
            { } v when v.HasFlag(Direction.Left) && v.HasFlag(Direction.Bottom)  => '\u2199',
            { } v when v.HasFlag(Direction.Left) && v.HasFlag(Direction.Top)     => '\u2196',
            var _                                                                => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }
}
