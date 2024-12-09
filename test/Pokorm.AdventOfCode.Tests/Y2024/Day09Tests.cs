using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day09Tests : DayTestBase
{
    public Day09Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Parse()
    {
        Assert.Equal("0..111....22222", Day09.Parse("12345").ToString());
        Assert.Equal("00...111...2...333.44.5555.6666.777.888899", Day09.Parse("2333133121414131402").ToString());
    }

    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day09();

        var result = day.Solve("12345");

        Assert.Equal(60, result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day09();

        var result = day.Solve("2333133121414131402");

        Assert.Equal(1928, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day09();

        var result = day.Solve(TextForDay(day));

        Assert.Equal(6346871685398, result);
    }

   /* [Fact]
    public void PartTwo_Sample()
    {
        var day = new Day09();

        var result = day.SolveBonus(LinesFromSample(
                """
                ...
                """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day09();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }*/
}
