using Pokorm.AdventOfCode2023;

namespace AdventOfCode2023.Tests;

public class Day3Tests : DayTestBase
{
    [Fact]
    public void SampleOne()
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

        var result =  day.SolveAsync();

        Assert.Equal(4361, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day3(this.InputService);

        var result =  day.SolveAsync();

        Assert.True(result > 550053);
        Assert.True(result <= 601575);
        Assert.True(result < 552575);
        Assert.Equal(551094, result);
    }

    [Fact]
    public void Sample2()
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

        var result =  day.SolveBonusAsync();

        Assert.Equal(467835, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day3(this.InputService);

        var result =  day.SolveBonusAsync();

        Assert.Equal(80179647, result);
    }
}
