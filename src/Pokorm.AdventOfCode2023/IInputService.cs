namespace Pokorm.AdventOfCode2023;

public interface IInputService
{
    Task<string> GetOrDownloadInputAsync(int day);
}