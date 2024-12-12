using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day12Tests : DayTestBase
{
    public Day12Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day12();

        var result = day.Solve(LinesFromSample(
            """
            AAAA
            BBCD
            BBCC
            EEEC
            """));

        Assert.Equal(140, result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day12();

        var result = day.Solve(LinesFromSample(
            """
            OOOOO
            OXOXO
            OOOOO
            OXOXO
            OOOOO
            """));

        Assert.Equal(772, result);
    }

    [Fact]
    public void PartOne_Sample_3()
    {
        var day = new Day12();

        var result = day.Solve(LinesFromSample(
            """
            RRRRIICCFF
            RRRRIICCCF
            VVRRRCCFFF
            VVRCCCJFFF
            VVVVCJJCFE
            VVIVCCJJEE
            VVIIICJJEE
            MIIIIIJJEE
            MIIISIJEEE
            MMMISSJEEE
            """));

        Assert.Equal(1930, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day12();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(1375476, result);
    }

    [Fact]
    public void PartTwo_Sample_1()
    {
        var day = new Day12();

        var result = day.SolveBonus(LinesFromSample(
            """
            AAAA
            BBCD
            BBCC
            EEEC
            """));

        Assert.Equal(80, result);
    }

    [Fact]
    public void PartTwo_Sample_2()
    {
        var day = new Day12();

        var result = day.SolveBonus(LinesFromSample(
            """
            EEEEE
            EXXXX
            EEEEE
            EXXXX
            EEEEE
            """));

        Assert.Equal(236, result);
    }

    [Fact]
    public void PartTwo_Sample_3()
    {
        var day = new Day12();

        var result = day.SolveBonus(LinesFromSample(
            """
            AAAAAA
            AAABBA
            AAABBA
            ABBAAA
            ABBAAA
            AAAAAA
            """));

        Assert.Equal(368, result);
    }

    [Fact]
    public void PartTwo_Sample_4()
    {
        var day = new Day12();

        var result = day.SolveBonus(LinesFromSample(
            """
            RRRRIICCFF
            RRRRIICCCF
            VVRRRCCFFF
            VVRCCCJFFF
            VVVVCJJCFE
            VVIVCCJJEE
            VVIIICJJEE
            MIIIIIJJEE
            MIIISIJEEE
            MMMISSJEEE
            """));

        Assert.Equal(1206, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day12();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(821372, result);
    }
}
