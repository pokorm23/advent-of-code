using Pokorm.AdventOfCode.Y2023.Days;

namespace Pokorm.AdventOfCode.Tests.Y2023;

public class Day08Tests : DayTestBase
{
    public Day08Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = new Day08(InputFromSample(
            """
            RL

            AAA = (BBB, CCC)
            BBB = (DDD, EEE)
            CCC = (ZZZ, GGG)
            DDD = (DDD, DDD)
            EEE = (EEE, EEE)
            GGG = (GGG, GGG)
            ZZZ = (ZZZ, ZZZ)
            """));

        var result = day.Solve();

        Assert.Equal(2, result);
    }

    [Fact]
    public void SampleTwo()
    {
        var day = new Day08(InputFromSample(
            """
            LLR
            
            AAA = (BBB, BBB)
            BBB = (AAA, ZZZ)
            ZZZ = (ZZZ, ZZZ)
            """));

        var result = day.Solve();

        Assert.Equal(6, result);
    }

    //[Fact]
    public void PartOne()
    {
        var day = new Day08(this.InputService);

        var result = day.Solve();

        Assert.Equal(249748283, result);
    }

    //[Fact]
    public void SampleThree()
    {
        var day = new Day08(InputFromSample(
            """

            """));

        var result = day.SolveBonus();

        Assert.Equal(71503, result);
    }

    //[Fact]
    public void PartTwo()
    {
        var day = new Day08(this.InputService);

        var result = day.SolveBonus();

        Assert.Equal(43663323, result);
    }
}
