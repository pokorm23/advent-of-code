namespace Pokorm.AdventOfCode2023;

public class AdventOfCodeOptions
{
    public int Year { get; set; } = 2023;

    public Uri BaseUrl { get; set; } = new Uri("https://adventofcode.com");

    public Uri GetDayUrl(int day) => new Uri(this.BaseUrl, $"{this.Year}/day/{day}");

    public Uri GetDayInputUrl(int day) => new Uri(this.BaseUrl, $"{this.Year}/day/{day}/input");
}
