namespace Pokorm.AdventOfCode2023;

public interface IDay
{
    int Day { get; }

    Task<string> SolveAsync();

    Task<string> SolveBonusAsync();
}
