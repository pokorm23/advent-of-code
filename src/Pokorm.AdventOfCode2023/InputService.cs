using Microsoft.Extensions.Hosting;

namespace Pokorm.AdventOfCode2023;

public class InputService : IInputService
{
    private readonly IHostEnvironment hostEnvironment;

    public InputService(IHostEnvironment hostEnvironment) => this.hostEnvironment = hostEnvironment;

    public string GetInput(int day)
    {
        var path = GetInputPath(day);

        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }

        throw new Exception($"{path} not found");
    }

    private string GetInputPath(int day)
    {
        var path = Path.Combine(this.hostEnvironment.ContentRootPath, $"Inputs/{day}.txt");

        return path;
    }
}

public interface IInputService
{
    string GetInput(int day);
}

public static class InputServiceExtensions
{
    public static string[] GetInputLines(this IInputService inputService, int day)
    {
        var input = inputService.GetInput(day);

        return input.Split(new[]
        {
            '\r',
            '\n'
        }, StringSplitOptions.RemoveEmptyEntries);
    }
}