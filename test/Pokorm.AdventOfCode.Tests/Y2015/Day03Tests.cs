using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day03Tests : DayTestBase
{
    public Day03Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne_1()
    {
        var day = new Day03();

        var result = day.Solve(">");

        Assert.Equal(2, result);
    }

    [Fact]
    public void SampleOne_2()
    {
        var day = new Day03();

        var result = day.Solve("^>v<");

        Assert.Equal(4, result);
    }

    [Fact]
    public void SampleOne_3()
    {
        var day = new Day03();

        var result = day.Solve("^v^v^v^v^v");

        Assert.Equal(2, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day03();

        var result = day.Solve(TextForDay(day));

        Assert.Equal(2081, result);
    }

    [Fact]
    public void SampleTwo_1()
    {
        var day = new Day03();

        var result = day.SolveBonus("^v");

        Assert.Equal(3, result);
    }

    [Fact]
    public void SampleTwo_2()
    {
        var day = new Day03();

        var result = day.SolveBonus("^>v<");

        Assert.Equal(3, result);
    }

    [Fact]
    public void SampleTwo_3()
    {
        var day = new Day03();

        var result = day.SolveBonus("^v^v^v^v^v");

        Assert.Equal(11, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day03();

        var result = day.SolveBonus(TextForDay(day));

        Assert.Equal(2341, result);
    }
}
