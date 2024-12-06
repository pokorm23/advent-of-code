using Pokorm.AdventOfCode.Y2023.Days;

namespace Pokorm.AdventOfCode.Tests.Y2023;

public class Day06Tests : DayTestBase
{
    public Day06Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = new Day06(InputFromSample(
                                """
                                Time:      7  15   30
                                Distance:  9  40  200
                                """));

        var result = day.Solve();

        Assert.Equal(288, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day06(this.InputService);

        var result = day.Solve();

        Assert.Equal(625968, result);
    }

    [Fact]
    public void SampleTwo()
    {
        var day = new Day06(InputFromSample(
            """
            Time:      7  15   30
            Distance:  9  40  200
            """));

        var result = day.SolveBonus();

        Assert.Equal(71503, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day06(this.InputService);

        var result = day.SolveBonus();

        Assert.Equal(43663323, result);
    }
}
