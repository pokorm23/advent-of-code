namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day01 : IDay
{
    private readonly IInputService inputService;

    public Day01(IInputService inputService) => this.inputService = inputService;

    public int SolveAsync()
    {
        var lines = this.inputService.GetInputLines(2023, 1);

        var sum = 0;

        foreach (var line in lines)
        {
            int? firstDigit = null;
            int? secondDigit = null;

            foreach (var c in line)
            {
                if (!char.IsDigit(c))
                {
                    continue;
                }

                if (firstDigit is null)
                {
                    firstDigit = int.Parse(c.ToString());

                    continue;
                }

                secondDigit = int.Parse(c.ToString());
            }

            secondDigit ??= firstDigit;

            var lineSum = 0;

            if (firstDigit is not null)
            {
                lineSum = firstDigit.Value * 10;
            }

            if (secondDigit is not null)
            {
                lineSum += secondDigit.Value;
            }

            sum += lineSum;
        }

        return sum;
    }

    public int SolveBonusAsync()
    {
        var lines = this.inputService.GetInputLines(2023, 1);

        var digitTexts = new Dictionary<string, int>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9
        };

        foreach (var c in "123456789")
        {
            digitTexts.Add(c.ToString(), int.Parse(c.ToString()));
        }

        var sum = 0;

        foreach (var line in lines)
        {
            var firstDigit = digitTexts.Select(x => (Value: (int?) x.Value, line.IndexOf(x.Key, StringComparison.OrdinalIgnoreCase)))
                                       .Where(x => x.Item2 >= 0)
                                       .MinBy(x => x.Item2).Value;

            var secondDigit = digitTexts.Select(x => (Value: (int?) x.Value, line.LastIndexOf(x.Key, StringComparison.OrdinalIgnoreCase)))
                                        .Where(x => x.Item2 >= 0)
                                        .MaxBy(x => x.Item2).Value;

            secondDigit ??= firstDigit;

            var lineSum = 0;

            if (firstDigit is not null)
            {
                lineSum = firstDigit.Value * 10;
            }

            if (secondDigit is not null)
            {
                lineSum += secondDigit.Value;
            }

            sum += lineSum;
        }

        return sum;
    }
}
