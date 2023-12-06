using System.Text;

namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day05 : IDay
{
    private readonly IInputService inputService;

    public Day05(IInputService inputService) => this.inputService = inputService;

    public int SolveAsync()
    {
        var data = Parse();

        return 0;
    }

    public int SolveBonusAsync()
    {
        var data = Parse();

        return 0;
    }

    private object Parse()
    {
        var lines = this.inputService.GetInputLines(2023, 5);
        return new object();
    }
}
