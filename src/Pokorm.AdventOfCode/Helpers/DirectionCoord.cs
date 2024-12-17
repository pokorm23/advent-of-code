namespace Pokorm.AdventOfCode.Helpers;

public record struct DirectionCoord(Coord Coord, Vector Direction)
{
    public Coord PreviousCoord { get; init; } 
}
