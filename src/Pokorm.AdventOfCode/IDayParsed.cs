namespace Pokorm.AdventOfCode;

public interface IDayParsed : IDay
{
    long Solve(string[] lines);

    long SolveBonus(string[] lines);

    long IDay.Solve() => Solve([ ]);

    long IDay.SolveBonus() => SolveBonus([ ]);
}
