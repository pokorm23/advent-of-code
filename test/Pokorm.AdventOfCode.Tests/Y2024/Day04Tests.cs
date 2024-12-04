using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day04Tests : DayTestBase
{
    public Day04Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = new Day04();

        var result = day.Solve(LinesFromSample(
            """
            MMMSXXMASM
            MSAMXMSMSA
            AMXSXMAAMM
            MSAMASMSMX
            XMASAMXAMM
            XXAMMXXAMA
            SMSMSASXSS
            SAXAMASAAA
            MAMMMXMMMM
            MXMXAXMASX
            """));

        Assert.Equal(18, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day04();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(2603, result);
    }

   /* [Fact]
    public void SampleBonus()
    {
        var day = new Day04();

        var result = day.SolveBonus(LinesFromSample(
            """
            xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))
            """));

        Assert.Equal(48, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day04();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(76911921, result);
    }*/
}
