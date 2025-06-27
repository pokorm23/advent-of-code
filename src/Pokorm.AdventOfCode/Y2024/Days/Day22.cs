namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/22
public class Day22(ILogger<Day22> logger)
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var result = 0L;

        foreach (var secret in data.Secrets)
        {
            var lastSecret = GetNextSecret(secret, 2000);

            result += lastSecret;
        }

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
    }

    public static long GetNextSecret(long secret, int iterations)
    {
        for (var i = 0; i < iterations; i++)
        {
            secret = GetNextSecret(secret);
        }

        return secret;
    }

    public static long GetNextSecret(long secret)
    {
        secret = Prune(Mix(secret * 64, secret));
        secret = Prune(Mix(secret / 32, secret));
        secret = Prune(Mix(secret * 2048, secret));

        return secret;
    }

    private static long Mix(long value, long secret) => value ^ secret;

    private static long Prune(long secret) => secret % 16777216;

    private static DayData Parse(string[] lines)
    {
        var secrets = lines.Select(x => long.Parse(x));

        return new DayData(secrets.ToList());
    }

    private record DayData(List<long> Secrets) { }
}
