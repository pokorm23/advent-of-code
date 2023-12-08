using Pokorm.AdventOfCode.Y2023.Days;

namespace Pokorm.AdventOfCode.Tests;

public class Day05Tests : DayTestBase
{
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

        var result = day.SolveAsync();

        Assert.Equal(35, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day05(this.InputService);

        var result = day.SolveAsync();

        Assert.Equal(313045984, result);
    }

    /*[Fact]
    public void Sample2()
    {
        var day = new Day05(InputFromSample(
            """
            Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
            Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
            Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
            Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
            Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
            Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
            """));

        var result = day.SolveBonusAsync();

        Assert.Equal(30, result);
    }*/

    /*[Fact]
    public void PartTwo()
    {
        var day = new Day05(this.InputService);

        var result = day.SolveBonusAsync();

        Assert.Equal(12648035, result);
    }*/
}
