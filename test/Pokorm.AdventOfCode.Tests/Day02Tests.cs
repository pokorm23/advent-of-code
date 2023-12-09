using Pokorm.AdventOfCode.Y2023.Days;
using Xunit.Abstractions;

namespace Pokorm.AdventOfCode.Tests;

public class Day02Tests : DayTestBase
{
    [Fact]
    public void SampleOne()
    {
        var day = new Day02(InputFromSample(
            """
            Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
            Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
            Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
            Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
            Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
            """));

        var result = day.Solve();

        Assert.Equal(8, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day02(this.InputService);

        var result = day.Solve();

        Assert.Equal(2551, result);
    }

    [Fact]
    public void Sample2()
    {
        var day = new Day02(InputFromSample(
            """
            Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
            Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
            Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
            Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
            Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
            """));

        var result = day.SolveBonus();

        Assert.Equal(2286, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day02(this.InputService);

        var result = day.SolveBonus();

        Assert.Equal(62811, result);
    }

    public Day02Tests(ITestOutputHelper output) : base(output) { }
}
