using Microsoft.Extensions.Hosting;

namespace Pokorm.AdventOfCode2023;

public class InputService : IInputService
{
    private readonly IHostEnvironment hostEnvironment;

    public InputService(IHostEnvironment hostEnvironment) => this.hostEnvironment = hostEnvironment;

    public async Task<string> GetInputAsync(int day)
    {
        var path = GetInputPath(day);

        if (File.Exists(path))
        {
            return await File.ReadAllTextAsync(path);
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
    Task<string> GetInputAsync(int day);
}

public static class InputServiceExtensions
{
    public static async Task<string[]> GetInputLinesAsync(this IInputService inputService, int day)
    {
        var input = await inputService.GetInputAsync(day);

        return input.Split(new[]
        {
            '\r',
            '\n'
        }, StringSplitOptions.RemoveEmptyEntries);
    }
}