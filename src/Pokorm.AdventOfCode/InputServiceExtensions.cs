namespace Pokorm.AdventOfCode;

public static class InputServiceExtensions
{
    public static string[] GetInputLines(this IInputService inputService, int year, int day)
    {
        var input = inputService.GetInput(year, day).ReplaceLineEndings();

        return input.Split(Environment.NewLine, StringSplitOptions.TrimEntries);
    }
    
    public static string[] GetInputLines(this IInputService inputService, Type type)
    {
        var year = int.Parse(type.Namespace?.Split('.').FirstOrDefault(x => x.StartsWith("Y"))?.Replace("Y", string.Empty));
        var day = int.Parse(type.Name.Replace("Day", string.Empty));
        
        var input = inputService.GetInput(year, day).ReplaceLineEndings();

        return input.Split(Environment.NewLine, StringSplitOptions.TrimEntries);
    }
}
