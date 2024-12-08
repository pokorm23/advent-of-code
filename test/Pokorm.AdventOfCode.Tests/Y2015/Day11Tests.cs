using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day11Tests : DayTestBase
{
    public Day11Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Next()
    {
        var day = new Day11();

        Assert.Equal("xz", day.GetNextPassword("xy"));
        Assert.Equal("ad", day.GetNextPassword("ac"));
        Assert.Equal("aa", day.GetNextPassword("zz"));
        Assert.Equal("abaa", day.GetNextPassword("aazz"));
    }

    [Fact]
    public void PartOne_IsValid()
    {
        var day = new Day11();

        Assert.False(day.IsValid("hijklmmn"));
        Assert.False(day.IsValid("abbceffg"));
        Assert.False(day.IsValid("abbcegjk"));
    }

    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day11();

        var result = day.Solve("abcdefgh");

        Assert.Equal("abcdffaa", result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day11();

        var result = day.Solve("ghijklmn");

        Assert.Equal("ghjaabcc", result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day11();

        var result = day.Solve(TextForDay(day));

        Assert.Equal("cqjxxyzz", result);
    }

    /*[Fact]
    public void PartTwo_Sample()
    {
        var day = new Day11();

        var result = day.SolveBonus(LinesFromSample(
                """
                ...
                """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day11();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }*/
}
