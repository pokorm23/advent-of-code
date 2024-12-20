using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day05Tests : DayTestBase
{
    public Day05Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day05();

        var result = day.Solve([ "ugknbfddgicrmopn" ]);

        Assert.Equal(1, result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day05();

        var result = day.Solve([ "aaa" ]);

        Assert.Equal(1, result);
    }

    [Fact]
    public void PartOne_Sample_3()
    {
        var day = new Day05();

        var result = day.Solve([ "jchzalrnumimnmhp" ]);

        Assert.Equal(0, result);
    }

    [Fact]
    public void PartOne_Sample_4()
    {
        var day = new Day05();

        var result = day.Solve([ "haegwjzuvuyypxyu" ]);

        Assert.Equal(0, result);
    }

    [Fact]
    public void PartOne_Sample_5()
    {
        var day = new Day05();

        var result = day.Solve([ "dvszwmarrgswjxmb" ]);

        Assert.Equal(0, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day05();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(238, result);
    }

    [Fact]
    public void PartTwo_Sample_1()
    {
        var day = new Day05();

        var result = day.SolveBonus([ "qjhvhtzxzqqjkmpb" ]);

        Assert.Equal(1, result);
    }

    [Fact]
    public void PartTwo_Sample_2()
    {
        var day = new Day05();

        var result = day.SolveBonus([ "xxyxx" ]);

        Assert.Equal(1, result);
    }

    [Fact]
    public void PartTwo_Sample_3()
    {
        var day = new Day05();

        var result = day.SolveBonus([ "uurcxstgmygtbstg" ]);

        Assert.Equal(0, result);
    }

    [Fact]
    public void PartTwo_Sample_4()
    {
        var day = new Day05();

        var result = day.SolveBonus([ "ieodomkazucvgmuy" ]);

        Assert.Equal(0, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day05();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(69, result);
    }
}
