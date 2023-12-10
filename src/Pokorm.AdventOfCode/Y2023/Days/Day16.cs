using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day16 : IDay
{
    private readonly IInputService inputService;

    public Day16(IInputService inputService) => this.inputService = inputService;

    public long Solve()
    {
        var data = Parse();

        return 0;
    }

    public long SolveBonus()
    {
        var data = Parse();

        return 0;
    }

    private Data Parse()
    {
        var lines = this.inputService.GetInputLines(GetType());

        foreach (var line in lines) { }

        var result = new Data();

        Trace.WriteLine($"Parsed: {result}");

        return result;
    }

    private record Data;
}
