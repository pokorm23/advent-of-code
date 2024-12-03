using System.Collections.Concurrent;
using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/1
public class Day01
{
    public long Solve(string[] lines)
    {
        List<int> firstColumn = [ ];
        List<int> secondColumn = [ ];

        foreach (var line in lines)
        {
            var rowSplit = line.FullSplit(' ');

            Debug.Assert(rowSplit.Length == 2);

            firstColumn.Add(int.Parse(rowSplit[0]));
            secondColumn.Add(int.Parse(rowSplit[1]));
        }

        firstColumn.Sort();
        secondColumn.Sort();

        var sum = 0;

        foreach (var (x, y) in firstColumn.Zip(secondColumn))
        {
            var d = Math.Abs(x - y);

            sum += d;
        }

        return sum;
    }

    public long SolveBonus(string[] lines)
    {
        List<int> firstColumn = [ ];
        ConcurrentDictionary<int, int> occurences = [ ];

        foreach (var line in lines)
        {
            var rowSplit = line.FullSplit(' ');

            Debug.Assert(rowSplit.Length == 2);

            firstColumn.Add(int.Parse(rowSplit[0]));

            occurences.AddOrUpdate(int.Parse(rowSplit[1]), 1, (key, value) => value + 1);
        }

        var sum = 0;

        foreach (var x in firstColumn)
        {
            sum += x * (occurences.TryGetValue(x, out var o) ? o : 0);
        }

        return sum;
    }
}
