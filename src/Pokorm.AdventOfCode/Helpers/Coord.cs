namespace Pokorm.AdventOfCode.Helpers;

internal record struct Coord(int X, int Y)
{
    public static Coord operator +(Coord c, Vector v) => new Coord(c.X + v.X, c.Y + v.Y);

    public static Coord operator -(Coord c, Vector v) => new Coord(c.X - v.X, c.Y - v.Y);

    public static Vector operator -(Coord c, Coord v) => new Vector(c.X - v.X, c.Y - v.Y);

    public override string ToString() => $"[{this.X},{this.Y}]";
}
