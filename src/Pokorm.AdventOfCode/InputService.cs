using Microsoft.Extensions.Hosting;

namespace Pokorm.AdventOfCode;

public class InputService : IInputService
{
    private readonly IHostEnvironment hostEnvironment;

    public InputService(IHostEnvironment hostEnvironment) => this.hostEnvironment = hostEnvironment;

    public string GetInput(int year, int day)
    {
        var path = GetInputPath(year, day);

        if (File.Exists(path))
        {
            return File.ReadAllText(path).TrimEnd('\r', '\n');
        }

        throw new Exception($"{path} not found");
    }

    private string GetInputPath(int year, int day)
    {
        var dayPrefix = day < 10 ? $"0{day}" : $"{day}";
        var path = Path.Combine(this.hostEnvironment.ContentRootPath, $"Y{year}/Inputs/{dayPrefix}.txt");

        return path;
    }
}
