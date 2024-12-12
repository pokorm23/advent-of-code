namespace Pokorm.AdventOfCode.Helpers;

public record Grid(int Width, int Height)
{
    public bool IsIn(Coord source) => source.Y >= 0 && source.X >= 0 && source.Y < this.Height && source.X < this.Width;

    public Coord? TryGetCoordInDirection(Coord source, Vector vector)
    {
        if (!IsIn(source))
        {
            throw new ArgumentOutOfRangeException(nameof(source), $"{source} range out in {this}");
        }

        var newCoord = new Coord(source.X + vector.X, source.Y + vector.Y);

        if (IsIn(newCoord))
        {
            return newCoord;
        }

        return null;
    }

    public override string ToString() => $"{this.Width}x{this.Height}";
}
