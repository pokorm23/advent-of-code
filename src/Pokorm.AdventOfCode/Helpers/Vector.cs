namespace Pokorm.AdventOfCode.Helpers;

public record struct Vector(int X, int Y)
{
    public Vector((int, int) tuple) : this(tuple.Item1, tuple.Item2) { }

    public static Vector Zero = new Vector(0, 0);

    public static Vector operator -(Vector c) => c * -1;

    public static Vector operator *(Vector c, int scale) => new Vector(c.X * scale, c.Y * scale);

    public static Vector operator *(int scale, Vector c) => c * scale;

    public string GetOrientationText() => (this.X, this.Y) switch
    {
        (0, 0)     => "\ud835\udfd8",
        (> 0, 0)   => "\u2192",
        (< 0, 0)   => "\u2190",
        (0, > 0)   => "\u2193",
        (0, < 0)   => "\u2193",
        (> 0, > 0) => "\u2198",
        (> 0, < 0) => "\u2197",
        (< 0, > 0) => "\u2199",
        (< 0, < 0) => "\u2196"
    };

    public override string ToString() => $"({this.X},{this.Y}) {GetOrientationText()}";

    public Vector ToRightRotated() => new Vector(this.Y, this.X);
}
