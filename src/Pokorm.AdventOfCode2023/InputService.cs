using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Pokorm.AdventOfCode2023;

public class InputService : IInputService
{
    private readonly IHostEnvironment hostEnvironment;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IOptions<AdventOfCodeOptions> options;

    public InputService(IOptions<AdventOfCodeOptions> options,
        IHttpClientFactory httpClientFactory,
        IHostEnvironment hostEnvironment)
    {
        this.options = options;
        this.httpClientFactory = httpClientFactory;
        this.hostEnvironment = hostEnvironment;
    }

    public async Task<string> GetOrDownloadInputAsync(int day)
    {
        var path = GetInputPath(day);
        
        if (File.Exists(path))
        {
            return await File.ReadAllTextAsync(path);
        }

        return await DownloadInputAsync(day);
    }

    public async Task<string> DownloadInputAsync(int day)
    {
        var content = await FetchInputAsync(day);

        var path = GetInputPath(day);

        await File.WriteAllTextAsync(path, content);

        return content;
    }

    public async Task<string> FetchInputAsync(int day)
    {
        var url = this.options.Value.GetDayInputUrl(day);

        using var client = this.httpClientFactory.CreateClient();

        Console.WriteLine($"Fetching {url} ...");

        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get input for day {day}.");
        }

        var content = await response.Content.ReadAsStringAsync();

        return content;
    }

    private string GetInputPath(int day)
    {
        var path = Path.Combine(this.hostEnvironment.ContentRootPath, $"Inputs/{day}.txt");

        return path;
    }
}