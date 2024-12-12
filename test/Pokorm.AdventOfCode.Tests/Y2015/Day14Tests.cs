using Microsoft.Extensions.Logging;
using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day14Tests : DayTestBase
{
    private readonly ILogger<Day14> logger;

    public Day14Tests(ILogger<Day14> logger) => this.logger = logger;

    [Fact]
    public void PartOne_Sample()
    {
        var day = new Day14(this.logger);

        var result = day.SolveIterations(LinesFromSample(
            """
            Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
            Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.
            """), 1000);

        Assert.Equal(1120, result.MaxDistance);
    }

    [Fact]
    public void PartOne_Samples()
    {
        var day = new Day14(this.logger);

        var result = day.SolveIterations(LinesFromSample(
            """
            Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
            Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.
            """), 1);

        Assert.Equal(16, result.MaxDistance);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day14(this.logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(2660, result);
    }

    [Fact]
    public void PartTwo_Sample()
    {
        var day = new Day14(this.logger);

        var result = day.SolveIterations(LinesFromSample(
            """
            Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
            Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.
            """), 1000);

        Assert.Equal(689, result.MaxPoints);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day14(this.logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(1256, result);
    }
}
