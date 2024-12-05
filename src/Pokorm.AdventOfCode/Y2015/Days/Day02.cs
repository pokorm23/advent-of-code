namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/2
public class Day02
{
    public static DayData Parse(string[] lines)
    {
        return new DayData(lines.Select(x =>
        {
            var nums = x.FullSplit('x')
                        .Select(int.Parse)
                        .ToArray();

            return new Dim(nums[0], nums[1], nums[2]);
        }).ToList());
    }

    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        return data.Dims.Sum(x => x.GetWrapArea());
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        return data.Dims.Sum(x => x.GetRibbonLength());
    }

    public record Dim(int Length, int Width, int Height)
    {
        public int GetWrapArea()
        {
            var smallest = Math.Min(this.Length * this.Width, Math.Min(this.Width * this.Height, this.Height * this.Length));

            return smallest + 2 * this.Length * this.Width + 2 * this.Width * this.Height + 2 * this.Height * this.Length;
        }

        public int GetRibbonLength()
        {
            var smallestSides = this.Length + this.Width + this.Height - Math.Max(this.Length, Math.Max(this.Width, this.Height));

            return 2 * smallestSides + this.Length * this.Width * this.Height;
        }
    }

    public record DayData(List<Dim> Dims) { }
}
