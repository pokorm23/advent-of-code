namespace Pokorm.AdventOfCode.Helpers;

[Flags]
public enum Direction
{
    Left = 1 << 0,
    Top = 1 << 1,
    Right = 1 << 2,
    Bottom = 1 << 3
}
