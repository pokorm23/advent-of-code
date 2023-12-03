namespace Pokorm.AdventOfCode2023;

public interface IDay
{
    int Day { get; }

    Task<int> SolveAsync();

    Task<int> SolveBonusAsync();
}
