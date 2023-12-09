using Pokorm.AdventOfCode.Y2023.Days;
using Xunit.Abstractions;

namespace Pokorm.AdventOfCode.Tests;

public class Day05Tests : DayTestBase
{
    public Day05Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void SampleOne()
    {
        var day = new Day05(InputFromSample(
            """
            seeds: 79 14 55 13

            seed-to-soil map:
            50 98 2
            52 50 48

            soil-to-fertilizer map:
            0 15 37
            37 52 2
            39 0 15

            fertilizer-to-water map:
            49 53 8
            0 11 42
            42 0 7
            57 7 4

            water-to-light map:
            88 18 7
            18 25 70

            light-to-temperature map:
            45 77 23
            81 45 19
            68 64 13

            temperature-to-humidity map:
            0 69 1
            1 0 69

            humidity-to-location map:
            60 56 37
            56 93 4
            """));

        var result = day.Solve();

        Assert.Equal(35, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day05(this.InputService);

        var result = day.Solve();

        Assert.Equal(313045984, result);
    }

    [Fact]
    public void SampleTwo()
    {
        // seeds: 79 14 55 13
        var day = new Day05(InputFromSample(
            """
            seeds: 79 14

            seed-to-soil map:
            50 98 2
            52 50 48

            soil-to-fertilizer map:
            0 15 37
            37 52 2
            39 0 15

            fertilizer-to-water map:
            49 53 8
            0 11 42
            42 0 7
            57 7 4

            water-to-light map:
            88 18 7
            18 25 70

            light-to-temperature map:
            45 77 23
            81 45 19
            68 64 13

            temperature-to-humidity map:
            0 69 1
            1 0 69

            humidity-to-location map:
            60 56 37
            56 93 4
            """));

        var result = day.SolveBonus();

        Assert.Equal(46, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day05(this.InputService);

        var result = day.SolveBonus();

        Assert.True(result < 26788781);
        Assert.Equal(20283860, result);
    }
}
