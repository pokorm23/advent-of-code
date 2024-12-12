using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/11
public class Day11
{
    private readonly ILogger<Day11> logger;

    public Day11(ILogger<Day11> logger) => this.logger = logger;

    public long Solve(string input) => SolveIterations(input, 25);

    public long SolveBonus(string input) => SolveIterations(input, 75);

    public long SolveIterations(string input, int iterations)
    {
        var origData = Parse(input);

        var stones = origData.Stones;

        for (var i = 0; i < iterations; i++)
        {
            //using var _ = this.logger.BeginScope($"{i + 1:00}:");

            //var sw = Stopwatch.GetTimestamp();

            stones = RunIteration(stones);

            //this.logger.LogDebug($"{Stopwatch.GetElapsedTime(sw).TotalMilliseconds:N4} ms -- {stones.Count} -- {stones.Sum(x => x.Value)} -- {string.Join(", ", stones.Select(x => $"{x.Key} ({x.Value})").ToList())}");
        }

        return stones.Sum(x => (long) x.Value);
    }

    private static DayData Parse(string input)
    {
        var stones = input.FullSplit(' ');

        var hash = new ConcurrentDictionary<long, long>();

        foreach (var s in stones)
        {
            hash.AddOrUpdate(long.Parse(s), _ => 1, (_, c) => c + 1);
        }

        return new DayData(hash);
    }

    public static ConcurrentDictionary<long, long> RunIteration(ConcurrentDictionary<long, long> initStones)
    {
        var stones = new ConcurrentDictionary<long, long>();

        foreach (var (stone, count) in initStones)
        {
            var str = stone.ToString();

            if (stone == 0)
            {
                stones.AddOrUpdate(1, _ => count, (_, c) => c + count);
            }
            else if (str.Length % 2 == 0)
            {
                var mid = str.Length / 2;

                var newOne = long.Parse(str[..mid]);
                var newOne2 = long.Parse(str[mid..]);

                stones.AddOrUpdate(newOne, _ => count, (_, c) => c + count);
                stones.AddOrUpdate(newOne2, _ => count, (_, c) => c + count);
            }
            else
            {
                var newStone = stone * 2024;

                stones.AddOrUpdate(newStone, _ => count, (_, c) => c + count);
            }
        }

        return stones;
    }

    public record DayData(ConcurrentDictionary<long, long> Stones)
    {
        public override string ToString()
        {
            return string.Join(" ", this.Stones.Select(x => x.ToString()));
        }
    }
}
