using Microsoft.Extensions.Logging;
using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day11Tests : DayTestBase
{
    private readonly ILogger<Day11> logger;

    public Day11Tests(ILogger<Day11> logger) => this.logger = logger;

    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day11(this.logger);

        var result = day.SolveIterations("0 1 10 99 999", 1);

        Assert.Equal(7, result.Stones.Count);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day11(this.logger);

        var input = "125 17";

        var result = day.SolveIterations(input, 1);

        Assert.Equal(3, result.Stones.Count);

        result = day.SolveIterations(input, 2);

        Assert.Equal(4, result.Stones.Count);

        result = day.SolveIterations(input, 3);

        Assert.Equal(5, result.Stones.Count);

        result = day.SolveIterations(input, 4);

        Assert.Equal(9, result.Stones.Count);

        result = day.SolveIterations(input, 5);

        Assert.Equal(13, result.Stones.Count);

        result = day.SolveIterations(input, 6);

        Assert.Equal(22, result.Stones.Count);

        result = day.SolveIterations(input, 25);

        Assert.Equal(55312, result.Stones.Count);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day11(this.logger);

        var result = day.Solve(TextForDay(day));

        Assert.Equal(218079, result);
    }

    [Fact]
    public void PartTwo_Speed()
    {
        /*int i;

        for (i = 42; ; i++)
        {
            var ct = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

            try
            {
                var day = new Day11(this.logger);
                day.SolveIterations("1", i, ct);
            }
            catch (OperationCanceledException e)
            {
                break;
            }
        }

        this.logger.LogInformation(i.ToString());
        Assert.True(i > 44);*/

        var day = new Day11(this.logger);
        //var ct = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
        day.SolveIterations("1", 60);

        /*
40:330.2299 ms
41:460.1497 ms
42:854.4925 ms
43:2,055.6628 ms
44:2,916.9291 ms
45:2,281.3691 ms
46:4,769.5901 ms
47:8,700.1197 ms
48:8,454.3051 ms
49:37,852.7656 ms
50:64,683.4760 ms
         */
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day11(this.logger);
        
        var result = day.SolveBonus(TextForDay(day));

        Assert.Equal(218079, result);
    }
}
