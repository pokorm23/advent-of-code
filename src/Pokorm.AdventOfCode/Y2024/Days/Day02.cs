namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/2
public class Day02
{
    public long Solve(string[] lines)
    {
        var safeCount = 0;

        foreach (var line in lines)
        {
            var rowSplit = line.FullSplit(' ');

            var levels = rowSplit.Select(int.Parse).ToList();

            if (!AreLevelsSafe(levels))
            {
                continue;
            }

            safeCount++;
        }

        return safeCount;
    }

    public long SolveBonus(string[] lines)
    {
        var safeCount = 0;

        foreach (var line in lines)
        {
            var rowSplit = line.FullSplit(' ');

            var levels = rowSplit.Select(int.Parse).ToList();

            if (AreLevelsSafe(levels))
            {
                safeCount++;
                continue;
            }

            for (var i = 0; i < levels.Count; i++)
            {
                var levelWithSkip = levels.ToList();

                levelWithSkip.RemoveAt(i);

                if (AreLevelsSafe(levelWithSkip))
                {
                    safeCount++;
                    break;
                }
            }
        }

        return safeCount;
    }

    bool AreLevelsSafe(IEnumerable<int> levels)
    {
        bool? isIncreasing = null;
        int? lastLevel = null;
        var success = true;

        foreach (var level in levels)
        {
            if (lastLevel is null)
            {
                lastLevel = level;

                continue;
            }

            if (Math.Abs(lastLevel.Value - level) is <= 0 or > 3)
            {
                success = false;

                break;
            }

            if (isIncreasing is null)
            {
                isIncreasing = level > lastLevel.Value;
                lastLevel = level;

                continue;
            }

            if (isIncreasing.Value && level < lastLevel.Value)
            {
                success = false;

                break;
            }

            if (!isIncreasing.Value && level > lastLevel.Value)
            {
                success = false;

                break;
            }

            lastLevel = level;
        }

        return success;
    }
}
