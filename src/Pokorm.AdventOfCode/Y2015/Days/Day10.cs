using System.Text;

namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/10
public class Day10
{
    public string SolveTimes(string input, int iterations)
    {
        for (var i = 0; i < iterations; i++)
        {
            char? lastChar = null;
            var currentSequenceCount = 0;
            var result = new StringBuilder();

            foreach (var c in input)
            {
                if (lastChar is null)
                {
                    lastChar = c;
                    currentSequenceCount = 1;

                    continue;
                }

                if (lastChar == c)
                {
                    currentSequenceCount++;

                    continue;
                }

                result.Append($"{currentSequenceCount}{lastChar}");

                currentSequenceCount = 1;

                lastChar = c;
            }

            if (currentSequenceCount >= 1 && lastChar is not null)
            {
                result.Append($"{currentSequenceCount}{lastChar}");
            }

            input = result.ToString();
        }

        return input;
    }

    public long Solve(string input) => SolveTimes(input, 40).Length;

    public long SolveBonus(string input) => SolveTimes(input, 50).Length;
}
