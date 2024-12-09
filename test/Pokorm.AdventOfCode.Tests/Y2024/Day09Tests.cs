using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day09Tests : DayTestBase
{
    private readonly ILogger<Day09> logger;

    public Day09Tests(ILogger<Day09> logger) => this.logger = logger;

    [Fact]
    public void PartOne_Parse()
    {
        Assert.Equal("0..111....22222", Day09.Parse("12345").ToString());
        Assert.Equal("00...111...2...333.44.5555.6666.777.888899", Day09.Parse("2333133121414131402").ToString());
    }

    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day09(this.logger);

        var result = day.Solve("12345");

        Assert.Equal(60, result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day09(this.logger);

        var result = day.Solve("2333133121414131402");

        Assert.Equal(1928, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day09(this.logger);

        var result = day.Solve(TextForDay(day));

        Assert.Equal(6346871685398, result);
    }

    [Fact]
    public void PartTwo_Sample_1()
    {
        var day = new Day09(this.logger);

        var result = day.SolveBonus("12345");

        Assert.Equal(132, result);
    }

    [Fact]
    public void PartTwo_Sample_2()
    {
        var day = new Day09(this.logger);

        var result = day.SolveBonus("2333133121414131402");

        Assert.Equal(2858, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day09(this.logger);

        var result = day.SolveBonus(TextForDay(day));

        Assert.True(result < 6373055449881);
        Assert.Equal(6373055193464, result);
    }
}
