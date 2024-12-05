namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/1
public class Day01
{
    public long Solve(string input)
    {
        var result = 0;

        foreach (var c in input)
        {
            if (c == '(')
            {
                result++;
            }

            if (c == ')')
            {
                result--;
            }
        }

        return result;
    }

    public long SolveBonus(string input)
    {
        var result = 0;

        foreach (var (i,c) in input.Index())
        {
            if (c == '(')
            {
                result++;
            }

            if (c == ')')
            {
                result--;
            }

            if (result == -1)
            {
                return i + 1;
            }
        }
        
        return result;
    }
}
