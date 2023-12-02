namespace Pokorm.AdventOfCode2023;

public class Day1 : IDay
{
    private readonly IInputService inputService;

    public Day1(IInputService inputService) => this.inputService = inputService;

    public int Day => 1;

    public async Task<string> SolveAsync()
    {
        var input = await this.inputService.GetOrDownloadInputAsync(this.Day);

        var lines = input.Split(new []{ '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

        var sum = 0;
        var i = 0;
        
        foreach (var line in lines)
        {
            i++;
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

        return sum.ToString();
    }

    public Task<string> SolveBonusAsync() => throw new NotImplementedException();
}