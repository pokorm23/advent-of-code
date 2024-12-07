using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day07Tests : DayTestBase
{
    public Day07Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample()
    {
        var day = new Day07();

        var result = day.Solve(LinesFromSample(
                """
                190: 10 19
                3267: 81 40 27
                83: 17 5
                156: 15 6
                7290: 6 8 6 15
                161011: 16 10 13
                192: 17 8 14
                21037: 9 7 18 13
                292: 11 6 16 20
                """));

        Assert.Equal(3749, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day07();

        var result = day.Solve(LinesForDay(day));

        Assert.True(result > 2314930696600);
        Assert.Equal(2314935962622, result);
    }

    [Fact]
    public void PartTwo_Sample()
    {
        var day = new Day07();

        var result = day.SolveBonus(LinesFromSample(
                """
                190: 10 19
                3267: 81 40 27
                83: 17 5
                156: 15 6
                7290: 6 8 6 15
                161011: 16 10 13
                192: 17 8 14
                21037: 9 7 18 13
                292: 11 6 16 20
                """));

        Assert.Equal(11387, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day07();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(401477450831495, result);
    }
}
