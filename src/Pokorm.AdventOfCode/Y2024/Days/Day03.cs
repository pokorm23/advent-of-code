using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/3
public partial class Day03
{
    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string MulOperationOperandRegexPattern = @"\d{1,3}";
    
    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string MulOperationRegexPattern = $@"mul\({MulOperationOperandRegexPattern},{MulOperationOperandRegexPattern}\)";
    
    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string EnableRegexPattern = @"do\(\)";
    
    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string DisableRegexPattern = @"don't\(\)";

    [GeneratedRegex(@"mul\((?<X>\d{1,3}),(?<Y>\d{1,3})\)")]
    public static partial Regex MulOperationRegexSimple();

    [GeneratedRegex($@"({MulOperationRegexPattern})|({EnableRegexPattern})|({DisableRegexPattern})")]
    public static partial Regex AllRegex();

    [GeneratedRegex(MulOperationRegexPattern)]
    public static partial Regex MulOperationRegex();

    [GeneratedRegex(MulOperationOperandRegexPattern)]
    public static partial Regex MulOperationOperandRegex();

    [GeneratedRegex(EnableRegexPattern)]
    public static partial Regex EnableRegex();

    [GeneratedRegex(DisableRegexPattern)]
    public static partial Regex DisableRegex();

    public long Solve(string[] lines)
    {
        var input = string.Join("", lines);

        var regex = MulOperationRegexSimple();

        var result = 0;

        foreach (var match in regex.Matches(input).AsEnumerable())
        {
            var x = int.Parse(match.Groups["X"].Value);
            var y = int.Parse(match.Groups["Y"].Value);
            var op = new MulOperation(x, y);
            result += op.Result;
        }

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var input = string.Join("", lines).AsSpan();

        var result = 0;
        var isEnabled = true;

        foreach (var match in AllRegex().EnumerateMatches(input))
        {
            var part = input.Slice(match.Index, match.Length);

            if (MulOperationRegex().IsMatch(part) && isEnabled)
            {
                var numberEn = MulOperationOperandRegex().EnumerateMatches(part).GetEnumerator();

                numberEn.MoveNext();
                var x = int.Parse(part.Slice(numberEn.Current.Index, numberEn.Current.Length));
                numberEn.MoveNext();
                var y = int.Parse(part.Slice(numberEn.Current.Index, numberEn.Current.Length));

                var op = new MulOperation(x, y);

                result += op.Result;
            }
            else if (EnableRegex().IsMatch(part))
            {
                isEnabled = true;
            }
            else if (DisableRegex().IsMatch(part))
            {
                isEnabled = false;
            }
        }

        return result;
    }

    private ref struct MulOperation
    {
        public int X { get; }

        public int Y { get; }

        public int Result => this.X * this.Y;

        public MulOperation(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
