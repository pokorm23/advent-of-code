namespace Pokorm.AdventOfCode.Helpers;

internal record struct Vector(int X, int Y)
{
    public Vector((int, int) tuple) : this(tuple.Item1, tuple.Item2) { }

    public static Vector Zero = new Vector(0, 0);

    public static Vector operator -(Vector c) => c * -1;

    public static Vector operator *(Vector c, int scale) => new Vector(c.X * scale, c.Y * scale);

    public override string ToString() => $"({this.X},{this.Y})";
}
