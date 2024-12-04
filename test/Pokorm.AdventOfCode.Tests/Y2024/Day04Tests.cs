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

   [Fact]
    public void SampleBonus()
    {
        var day = new Day04();

        var result = day.SolveBonus(LinesFromSample(
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

        Assert.Equal(9, result);
    }

     [Fact]
    public void PartTwo()
    {
        var day = new Day04();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(1965, result);
    }
}
