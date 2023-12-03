namespace Pokorm.AdventOfCode2023;

public interface IDay
{
    int Day { get; }

    int SolveAsync();

    int SolveBonusAsync();
}
