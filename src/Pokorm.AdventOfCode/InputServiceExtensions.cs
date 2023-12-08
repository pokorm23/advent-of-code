namespace Pokorm.AdventOfCode;

public static class InputServiceExtensions
{
    public static string[] GetInputLines(this IInputService inputService, int year, int day)
    {
        var input = inputService.GetInput(year, day).ReplaceLineEndings();

        return input.Split(Environment.NewLine, StringSplitOptions.TrimEntries);
    }
}
