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

        foreach (var line in lines)
        {
            char? lastChar = null;
            char? lastLastChar = null;
            var rule1 = false;
            var rule2 = false;
            var potentialPairs = new Dictionary<string, int>();

            foreach (var (i, c) in line.ToLower().Index())
            {
                if (rule1 && rule2)
                {
                    break;
                }

                if (lastChar is null)
                {
                    lastLastChar = lastChar;
                    lastChar = c;

                    continue;
                }

                var lastTwoChars = $"{lastChar.Value}{c}";

                if (potentialPairs.TryGetValue(lastTwoChars, out var firstIndex) && i - firstIndex > 2)
                {
                    rule1 = true;
                }

                potentialPairs.TryAdd(lastTwoChars, i - 1);

                if (lastLastChar is not null && lastLastChar.Value == c)
                {
                    rule2 = true;
                }

                lastLastChar = lastChar;
                lastChar = c;
            }

            if (rule1 && rule2)
            {
                result++;
            }
        }

        return result;
    }
}
