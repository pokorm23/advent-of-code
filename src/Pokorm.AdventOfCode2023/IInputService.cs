namespace Pokorm.AdventOfCode2023;

public interface IInputService
{
    Task<string> GetOrDownloadInputAsync(int day);

    Task<string> DownloadInputAsync(int day);

    Task<string> FetchInputAsync(int day);
}