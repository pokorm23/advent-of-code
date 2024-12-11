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

        /*using (_ = this.logger.BeginScope($"{0:00}:"))
        {
            this.logger.LogDebug(data.ToString());
        }*/
        var lookup = new Dictionary<Stone, List<Stone>>();

        lookup.Add(new Stone(0), [ new Stone(1) ]);

        for (var i = 0; i < iterations; i++)
        {
            //using var _ = this.logger.BeginScope($"{i + 1:00}:");

            data = data.RunIteration(lookup);

            //this.logger.LogDebug(data.ToString());
        }

        //this.logger.LogDebug("----------");

        return data;
    }

    private static DayData Parse(string input)
    {
        var stones = input.FullSplit(' ');

        var hash = new List<Stone>();

        foreach (var s in stones)
        {
            hash.Add(new Stone(long.Parse(s)));
        }

        return new DayData(hash);
    }

    public record struct Stone(long Value) { }

    public record DayData(List<Stone> Stones)
    {
        public override string ToString()
        {
            return string.Join(" ", this.Stones.Select(x => x.ToString()));
        }

        public DayData RunIteration(Dictionary<Stone, List<Stone>> lookup)
        {
            var newStones = new List<Stone>();

            foreach (var stone in this.Stones)
            {
                if (lookup.TryGetValue(stone, out var look))
                {
                    newStones.AddRange(look);

                    continue;
                }

                var str = stone.Value.ToString();

                var newOnes = new List<Stone>();

                if (str.Length % 2 == 0)
                {
                    var mid = str.Length / 2;

                    newOnes.Add(new Stone(long.Parse(str[..mid])));
                    newOnes.Add(new Stone(long.Parse(str[mid..])));
                }
                else
                {
                    var newStone = stone.Value * 2024;
                    newOnes.Add(new Stone(newStone));
                }

                lookup.Add(stone, newOnes);

                newStones.AddRange(newOnes);
            }

            return new DayData(newStones);
        }
    }
}
