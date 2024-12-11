using System.Text;
using Microsoft.Extensions.Logging;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/11
public class Day11
{
    private readonly ILogger<Day11> logger;

    public Day11(ILogger<Day11> logger) => this.logger = logger;

    public long Solve(string input) => SolveIterations(input, 25).Stones.Count;

    public long SolveBonus(string input) => 0;

    public DayData SolveIterations(string input, int iterations)
    {
        var origData = Parse(input);

        var data = origData;

        using (_ = this.logger.BeginScope($"{0:00}:"))
        {
            this.logger.LogDebug(data.ToString());
        }

        for (var i = 0; i < iterations; i++)
        {
            using var _ = this.logger.BeginScope($"{i + 1:00}:");

            data = data.RunIteration();

            this.logger.LogDebug(data.ToString());
        }

        this.logger.LogDebug("----------");

        return data;
    }

    private static DayData Parse(string input)
    {
        var stones = input.FullSplit(' ');

        var hash = new SortedSet<Stone>();

        var stoneStr = new StringBuilder();

        var i = 0;

        foreach (var s in stones)
        {
            hash.Add(new Stone(i)
            {
                Length = s.Length
            });

            stoneStr.Append(s);

            i += s.Length;
        }

        return new DayData(stoneStr.ToString(), hash);
    }

    public record struct Stone(int Offset) : IComparable<Stone>, IComparable
    {
        public int CompareTo(Stone other) => this.Offset.CompareTo(other.Offset);

        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            return obj is Stone other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Stone)}");
        }

        public required int Length { get; init; }

        public Range Range => new Range(new Index(this.Offset), new Index(this.Offset + this.Length));
    }

    public record DayData(string Text, SortedSet<Stone> Stones)
    {
        public override string ToString()
        {
            var result = new List<long>();

            foreach (var stone in this.Stones)
            {
                var seg = this.Text[stone.Range];

                result.Add(long.Parse(seg));
            }

            return string.Join(" ", result.Select(x => x.ToString()));
        }

        public DayData RunIteration()
        {
            var result = new StringBuilder(this.Text.Length);
            var newStones = new SortedSet<Stone>();

            var mem = this.Text.AsSpan();

            var accOffset = 0;

            foreach (var stone in this.Stones)
            {
                var seg = mem[stone.Range];

                if (seg.Length == 1 && seg[0] == '0')
                {
                    result.Append('1');

                    newStones.Add(stone with
                    {
                        Offset = stone.Offset + accOffset
                    });
                }
                else if (seg.Length % 2 == 0)
                {
                    var mid = stone.Length / 2;

                    var secondPart = seg[mid..].TrimStart('0');

                    if (secondPart.Length == 0)
                    {
                        secondPart = [ '0' ];
                    }

                    result.Append(seg[..mid]);
                    result.Append(secondPart);

                    newStones.Add(stone with
                    {
                        Offset = stone.Offset + accOffset,
                        Length = mid
                    });

                    newStones.Add(new Stone()
                    {
                        Offset = stone.Offset + mid + accOffset,
                        Length = secondPart.Length
                    });

                    accOffset += secondPart.Length - mid;
                }
                else
                {
                    var newStone = (long.Parse(seg) * 2024).ToString();

                    result.Append(newStone);

                    newStones.Add(stone with
                    {
                        Offset = stone.Offset + accOffset,
                        Length = newStone.Length
                    });

                    accOffset += newStone.Length - seg.Length;
                }
            }

            return new DayData(result.ToString(), newStones);
        }
    }
}
