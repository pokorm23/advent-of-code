using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Pokorm.AdventOfCode2023;

public class InputService : IInputService
{
    private readonly IHostEnvironment hostEnvironment;

    public InputService(IHostEnvironment hostEnvironment)
    {
        this.hostEnvironment = hostEnvironment;
    }

    public async Task<string> GetOrDownloadInputAsync(int day)
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