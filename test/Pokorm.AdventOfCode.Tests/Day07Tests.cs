using Pokorm.AdventOfCode.Y2023.Days;
using Xunit.Abstractions;

namespace Pokorm.AdventOfCode.Tests;

public class Day07Tests : DayTestBase
{
    public Day07Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = CreateDayFromSample(
            """
            32T3K 765
            T55J5 684
            KK677 28
            KTJJT 220
            QQQJA 483
            """);

        var result = day.Solve();

        Assert.Equal(6440, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = CreateDay();

        var result = day.Solve();

        Assert.Equal(249748283, result);
    }

    /*[Fact]
    public void SampleTwo()
    {
        var day = CreateDayFromSample(
            """
            Time:      7  15   30
            Distance:  9  40  200
            """);

        var result = day.SolveBonus();

        Assert.Equal(71503, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = CreateDay();

        var result = day.SolveBonus();

        Assert.Equal(43663323, result);
    }*/
}
