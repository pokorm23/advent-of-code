using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day06Tests : DayTestBase
{
    public Day06Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day06();

        var result = day.Solve([ "turn on 0,0 through 999,999" ]);

        Assert.Equal(1000*1000, result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day06();

        var result = day.Solve([ "toggle 0,0 through 999,0" ]);

        Assert.Equal(1000, result);
    }

    [Fact]
    public void PartOne_Sample_3()
    {
        var day = new Day06();

        var result = day.Solve([ "turn off 499,499 through 500,500" ]);

        Assert.Equal(0, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day06();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(400410, result);
    }

    [Fact]
    public void PartTwo_Sample_1()
    {
        var day = new Day06();

        var result = day.SolveBonus([ "qjhvhtzxzqqjkmpb" ]);

        Assert.Equal(1, result);
    }

    [Fact]
    public void PartTwo_Sample_2()
    {
        var day = new Day06();

        var result = day.SolveBonus([ "xxyxx" ]);

        Assert.Equal(1, result);
    }

    [Fact]
    public void PartTwo_Sample_3()
    {
        var day = new Day06();

        var result = day.SolveBonus([ "uurcxstgmygtbstg" ]);

        Assert.Equal(0, result);
    }

    [Fact]
    public void PartTwo_Sample_4()
    {
        var day = new Day06();

        var result = day.SolveBonus([ "ieodomkazucvgmuy" ]);

        Assert.Equal(0, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day06();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(69, result);
    }
}
