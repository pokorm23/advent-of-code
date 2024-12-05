using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day05Tests : DayTestBase
{
    public Day05Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = new Day05();

        var result = day.Solve(LinesFromSample(
            """
            47|53
            97|13
            97|61
            97|47
            75|29
            61|13
            75|53
            29|13
            97|29
            53|29
            61|53
            97|53
            61|29
            47|13
            75|47
            97|75
            47|61
            75|61
            47|29
            75|13
            53|13

            75,47,61,53,29
            97,61,53,29,13
            75,29,13
            75,97,47,61,53
            61,13,29
            97,13,75,29,47
            """));

        Assert.Equal(143, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day05();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(4996, result);
    }

    [Fact]
    public void SampleBonus()
    {
        var day = new Day05();

        var result = day.SolveBonus(LinesFromSample(
            """
            47|53
            97|13
            97|61
            97|47
            75|29
            61|13
            75|53
            29|13
            97|29
            53|29
            61|53
            97|53
            61|29
            47|13
            75|47
            97|75
            47|61
            75|61
            47|29
            75|13
            53|13
            
            75,47,61,53,29
            97,61,53,29,13
            75,29,13
            75,97,47,61,53
            61,13,29
            97,13,75,29,47
            """));

        Assert.Equal(123, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day05();

        var result = day.SolveBonus(LinesForDay(day));

        Assert.True(result < 11155);
        Assert.True(result > 4905);
        Assert.True(result < 10977);
        Assert.Equal(6311, result);
    }
}
