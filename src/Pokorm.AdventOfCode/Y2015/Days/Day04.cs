using System.Security.Cryptography;
using System.Text;

namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/4
public class Day04
{
    public long Solve(string input) => Solve(input, 5);

    public long SolveBonus(string input) => Solve(input, 6);

    private long Solve(string input, int numberOfZeros)
    {
        using var md5 = MD5.Create();
        var pivot = new string('0', numberOfZeros);

        for (var i = 0;; i++)
        {
            var append = i == 0 ? "" : i.ToString();

            var bytes = Encoding.UTF8.GetBytes(input + append);

            var resultBytes = md5.ComputeHash(bytes);

            var hash = Convert.ToHexString(resultBytes);

            if (hash.StartsWith(pivot))
            {
                return i;
            }
        }
    }
}
