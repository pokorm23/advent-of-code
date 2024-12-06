namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/5
public class Day05
{
    public long Solve(string[] lines)
    {
        var result = 0;

        foreach (var line in lines)
        {
            char? lastChar = null;
            var containsTwoInRow = false;
            var hasBadSubstring = false;
            var seenVowels = new List<char>();

            foreach (var c in line.ToLower())
            {
                if (c is 'a' or 'e' or 'i' or 'o' or 'u')
                {
                    seenVowels.Add(c);
                }

                if (lastChar is not null && c == lastChar)
                {
                    containsTwoInRow = true;
                }

                if (lastChar is not null && $"{lastChar.Value}{c}" is "ab" or "cd" or "pq" or "xy")
                {
                    hasBadSubstring = true;
                }

                lastChar = c;
            }

            if (seenVowels.Count >= 3 && !hasBadSubstring && containsTwoInRow)
            {
                result++;
            }
        }

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var result = 0;

        return result;
    }
}
