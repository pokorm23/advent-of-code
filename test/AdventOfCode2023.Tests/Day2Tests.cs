using Pokorm.AdventOfCode2023;

namespace AdventOfCode2023.Tests;

public class Day2Tests : DayTestBase
{
    [Fact]
    public async Task SampleOne()
    {
        var day = new Day2(InputFromSample(
            """
            Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
            Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
            Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
            Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
            Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
            """));

        var result = await day.SolveAsync();

        Assert.Equal("8", result);
    }

    [Fact]
    public async Task PartOne()
    {
        var day = new Day2(this.InputService);

        var result = await day.SolveAsync();

        Assert.Equal("2207", result);
    }

    [Fact]
    public async Task Sample2()
    {
        var day = new Day2(InputFromSample(
            """
            Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
            Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
            Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
            Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
            Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
            """));

        var result = await day.SolveBonusAsync();

        Assert.Equal("2286", result);
    }

    [Fact]
    public async Task PartTwo()
    {
        throw new NotImplementedException();
    }
}
