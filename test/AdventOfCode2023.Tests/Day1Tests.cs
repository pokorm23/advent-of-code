using Pokorm.AdventOfCode2023;

namespace AdventOfCode2023.Tests;

public class Day1Tests : DayTestBase
{
    [Fact]
    public async Task SampleOne()
    {
        var day = new Day1(InputFromSample(
            """
            1abc2
            pqr3stu8vwx
            a1b2c3d4e5f
            treb7uchet
            """));

        var result = await day.SolveAsync();

        Assert.Equal(142, result);
    }

    [Fact]
    public async Task PartOne()
    {
        var day = new Day1(this.InputService);

        var result = await day.SolveAsync();

        Assert.Equal(53194, result);
    }

    [Fact]
    public async Task Sample2()
    {
        var day = new Day1(InputFromSample(
            """
            two1nine
            eightwothree
            abcone2threexyz
            xtwone3four
            4nineeightseven2
            zoneight234
            7pqrstsixteen
            """));

        var result = await day.SolveBonusAsync();

        Assert.Equal(281, result);
    }

    [Fact]
    public async Task PartTwo()
    {
        var day = new Day1(this.InputService);

        var result = await day.SolveBonusAsync();

        Assert.Equal(54249, result);
    }
}
