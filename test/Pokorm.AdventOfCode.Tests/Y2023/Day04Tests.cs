using Pokorm.AdventOfCode.Y2023.Days;

namespace Pokorm.AdventOfCode.Tests.Y2023;

public class Day04Tests : DayTestBase
{
    public Day04Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = new Day04(InputFromSample(
            """
            Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
            Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
            Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
            Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
            Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
            Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
            """));

        var result = day.Solve();

        Assert.Equal(13, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day04(this.InputService);

        var result = day.Solve();

        Assert.Equal(20829, result);
    }

    [Fact]
    public void Sample2()
    {
        var day = new Day04(InputFromSample(
            """
            Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
            Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
            Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
            Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
            Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
            Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
            """));

        var result = day.SolveBonus();

        Assert.Equal(30, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day04(this.InputService);

        var result = day.SolveBonus();

        Assert.Equal(12648035, result);
    }
}
