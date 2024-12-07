namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/8
public class Day08
{
    public long Solve(string[] lines)
    {
        var result = 0;

        foreach (var line in lines)
        {
            var codeLength = line.Length;
            var memoryLength = 0;
            var readingHex = -1;

            var isEscaping = false;

            foreach (var c in line)
            {
                if (c is '\\' && !isEscaping)
                {
                    isEscaping = true;

                    continue;
                }

                if (readingHex == 0)
                {
                    readingHex = 1;

                    continue;
                }

                if (readingHex == 1)
                {
                    readingHex = -1;

                    continue;
                }

                if (isEscaping)
                {
                    if (c is 'x')
                    {
                        readingHex = 0;
                    }
                    else if (c is not ('"' or '\\'))
                    {
                        memoryLength++; // we count the \
                    }
                }

                memoryLength++;
                isEscaping = false;
            }

            memoryLength -= 2; // ""

            result += codeLength - memoryLength;
        }

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var result = 0;

        return result;
    }
}
