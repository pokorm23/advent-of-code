using Pokorm.AdventOfCode.Y2023.Days;

namespace Pokorm.AdventOfCode.Tests;

public class Day01Tests : DayTestBase
{
    [Fact]
    public void SampleOne()
    {
        var day = new Day01(InputFromSample(
            """
            1abc2
            pqr3stu8vwx
            a1b2c3d4e5f
            treb7uchet
            """));

        var result = day.Solve();

        Assert.Equal(142, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day01(this.InputService);

        var result = day.Solve();

        Assert.Equal(53194, result);
    }

    [Fact]
    public void Sample2()
    {
        var day = new Day01(InputFromSample(
            """
            two1nine
            eightwothree
            abcone2threexyz
            xtwone3four
            4nineeightseven2
            zoneight234
            7pqrstsixteen
            """));

        var result = day.SolveBonus();

        Assert.Equal(281, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day01(this.InputService);

        var result = day.SolveBonus();

        Assert.Equal(54249, result);
    }
}
