namespace Pokorm.AdventOfCode.Helpers;

public record struct Vector(long X, long Y)
{
    public Vector((long, long) tuple) : this(tuple.Item1, tuple.Item2) { }

    public static Vector Zero = new Vector(0, 0);

    public static Vector Top = new Vector(0, 1);

    public static Vector TopRight = new Vector(1, 1);

    public static Vector TopLeft = new Vector(-1, 1);

    public static Vector Left = new Vector(-1, 0);

    public static Vector Bottom = new Vector(0, -1);

    public static Vector BottomLeft = new Vector(-1, -1);

    public static Vector BottomRigth = new Vector(1, -1);

    public static Vector Right = new Vector(1, 0);

    public static Vector[] Horizontal = [ Left, Right ];

    public static Vector[] Vertical = [ Top, Bottom ];

    public static Vector[] Diagonal = [ TopRight, TopLeft, BottomRigth, BottomLeft ];

    public static Vector[] Directional = [ ..Horizontal, ..Vertical ];

    public static Vector[] All = [ ..Directional, ..Diagonal ];

    public static Vector operator -(Vector c) => c * -1;

    public static Vector operator *(Vector c, long scale) => new Vector(c.X * scale, c.Y * scale);

    public static Vector operator *(long scale, Vector c) => c * scale;

    public string GetOrientationText() => (this.X, this.Y) switch
    {
        (0, 0)     => "\ud835\udfd8",
        (> 0, 0)   => "\u2192",
        (< 0, 0)   => "\u2190",
        (0, > 0)   => "\u2193",
        (0, < 0)   => "\u2191",
        (> 0, > 0) => "\u2198",
        (> 0, < 0) => "\u2197",
        (< 0, > 0) => "\u2199",
        (< 0, < 0) => "\u2196"
    };

    public Direction? ToDirection() => (this.X, this.Y) switch
    {
        (0, 0)     => null,
        (> 0, 0)   => Direction.Right,
        (< 0, 0)   => Direction.Left,
        (0, > 0)   => Direction.Bottom,
        (0, < 0)   => Direction.Top,
        (> 0, > 0) => Direction.Right | Direction.Bottom,
        (> 0, < 0) => Direction.Right | Direction.Top,
        (< 0, > 0) => Direction.Left | Direction.Bottom,
        (< 0, < 0) => Direction.Left | Direction.Top
    };

    public override string ToString() => $"({this.X},{this.Y}) {GetOrientationText()}";

    public Vector ToRightRotated() => new Vector(this.Y, this.X);
}
