namespace Pokorm.AdventOfCode.Helpers;

public static class HelperExtensions
{
    public static IEnumerable<Coord> GetSiblings(this Coord coord, IEnumerable<Vector> vectors, Grid grid)
    {
        return vectors.Select(v => grid.TryGetCoordInDirection(coord, v))
                      .OfType<Coord>();
    }
}
