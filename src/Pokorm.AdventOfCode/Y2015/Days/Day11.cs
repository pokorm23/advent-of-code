using System.Diagnostics;
using System.Text;

namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/11
public class Day11
{
    public string Solve(string input)
    {
        input = GetNextPassword(input);

        while (!IsValid(input))
        {
            input = GetNextPassword(input);
        }

        return input;
    }

    public bool IsValid(string input)
    {
        char? lc = null;
        char? llc = null;
        var rule1 = false;

        var rule2 = new HashSet<char>();

        foreach (var c in input)
        {
            if (c is 'i' or 'o' or 'l')
            {
                return false;
            }

            if (!rule1 && lc.HasValue && llc.HasValue)
            {
                rule1 = c - lc == 1 && lc - llc == 1;
            }

            if (rule2.Count < 2 && lc.HasValue && !rule2.Contains(c) && c == lc)
            {
                rule2.Add(c);
            }

            (lc, llc) = (c, lc);
        }

        return rule1 && rule2.Count >= 2;
    }

    public string GetNextPassword(string input)
    {
        Debug.Assert(input.Length > 0);

        var result = new StringBuilder();

        bool? wrap = false;

        foreach (var c in input.Reverse())
        {
            if (wrap is null)
            {
                result.Insert(0, c);

                continue;
            }

            var nc = c;

            if (c == 'z')
            {
                wrap = true;
                nc = 'a';
                result.Insert(0, nc);

                continue;
            }

            nc = (char) (nc + 1);
            wrap = null;

            result.Insert(0, nc);
        }

        return result.ToString();
    }

    public string SolveBonus(string input) => "";
}
