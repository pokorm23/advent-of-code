using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/11
public class Day11
{
    private readonly ILogger<Day11> logger;

    public Day11(ILogger<Day11> logger) => this.logger = logger;

    public long Solve(string input) => SolveIterations(input, 25).Stones.Count;

    public long SolveBonus(string input) => SolveIterations(input, 75).Stones.Count;

    public DayData SolveIterations(string input, int iterations)
    {
        var origData = Parse(input);

        var data = origData;

        var lookup = new Dictionary<long, (long, long?)>();

        for (var i = 0; i < iterations; i++)
        {
            lookup.Clear();
            lookup.Add(0, (1, null));

            using var _ = this.logger.BeginScope($"{i + 1:00}:");

            var sw = Stopwatch.GetTimestamp();

            data = data.RunIteration(lookup);

            this.logger.LogDebug($"{Stopwatch.GetElapsedTime(sw).TotalMilliseconds:N4} ms");
        }

        return data;
    }

    private static DayData Parse(string input)
    {
        var stones = input.FullSplit(' ');

        var hash = new List<long>();

        foreach (var s in stones)
        {
            hash.Add(long.Parse(s));
        }

        return new DayData(hash);
    }

    public record DayData(List<long> Stones)
    {
        public override string ToString()
        {
            return string.Join(" ", this.Stones.Select(x => x.ToString()));
        }

        public DayData RunIteration(Dictionary<long, (long, long?)> lookup)
        {
            var newStones = new List<long>(this.Stones.Capacity);

            foreach (var stone in this.Stones)
            {
                if (lookup.TryGetValue(stone, out var look))
                {
                    newStones.Add(look.Item1);

                    if (look.Item2.HasValue)
                    {
                        newStones.Add(look.Item2.Value);
                    }

                    continue;
                }

                var str = stone.ToString();

                long newOne;
                long? newOne2 = null;

                if (str.Length % 2 == 0)
                {
                    var mid = str.Length / 2;

                    newOne = long.Parse(str[..mid]);
                    newOne2 = long.Parse(str[mid..]);

                    newStones.Add(newOne);
                    newStones.Add(newOne2.Value);
                }
                else
                {
                    var newStone = stone * 2024;
                    newOne = newStone;
                    newStones.Add(newOne);
                }

                {
                    lookup.Add(stone, (newOne, newOne2));
                }
            }

            return new DayData(newStones);
        }
    }
}
