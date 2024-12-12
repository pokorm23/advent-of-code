using Microsoft.Extensions.Logging;
using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day13Tests : DayTestBase
{
    private readonly ILogger<Day13> logger;

    public Day13Tests(ILogger<Day13> logger) => this.logger = logger;

    [Fact]
    public void PartOne_Sample()
    {
        var day = new Day13(this.logger);

        var result = day.Solve(LinesFromSample(
            """
            Alice would gain 54 happiness units by sitting next to Bob.
            Alice would lose 79 happiness units by sitting next to Carol.
            Alice would lose 2 happiness units by sitting next to David.
            Bob would gain 83 happiness units by sitting next to Alice.
            Bob would lose 7 happiness units by sitting next to Carol.
            Bob would lose 63 happiness units by sitting next to David.
            Carol would lose 62 happiness units by sitting next to Alice.
            Carol would gain 60 happiness units by sitting next to Bob.
            Carol would gain 55 happiness units by sitting next to David.
            David would gain 46 happiness units by sitting next to Alice.
            David would lose 7 happiness units by sitting next to Bob.
            David would gain 41 happiness units by sitting next to Carol.
            """));

        Assert.Equal(330, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day13(this.logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(664, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day13(this.logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(640, result);
    }
}
