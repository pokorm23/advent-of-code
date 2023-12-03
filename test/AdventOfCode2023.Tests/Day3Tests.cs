using Pokorm.AdventOfCode2023;

namespace AdventOfCode2023.Tests;

public class Day3Tests : DayTestBase
{
    [Fact]
    public async Task SampleOne()
    {
        var day = new Day3(InputFromSample(
            """
            467..114..
            ...*......
            ..35..633.
            ......#...
            617*......
            .....+.58.
            ..592.....
            ......755.
            ...$...*..
            .664...598
            """));

        var result = await day.SolveAsync();

        Assert.Equal(4361, result);
    }

    [Fact]
    public async Task PartOne()
    {
        var day = new Day3(this.InputService);

        var result = await day.SolveAsync();

        Assert.True(result > 550053);
        Assert.True(result <= 601575);
        Assert.True(result < 552575);
        Assert.Equal(551094, result);
    }

    [Fact]
    public async Task Sample2()
    {
        var day = new Day3(InputFromSample(
            """
            467..114..
            ...*......
            ..35..633.
            ......#...
            617*......
            .....+.58.
            ..592.....
            ......755.
            ...$.*....
            .664.598..
            """));

        var result = await day.SolveBonusAsync();

        Assert.Equal(467835, result);
    }

    [Fact]
    public async Task PartTwo()
    {
        var day = new Day3(this.InputService);

        var result = await day.SolveBonusAsync();

    }
}
