using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day16Tests(ILogger<Day16> logger) : DayTestBase
{
    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day16(logger);

        var result = day.Solve(LinesFromSample(
            """
            ###############
            #.......#....E#
            #.#.###.#.###.#
            #.....#.#...#.#
            #.###.#####.#.#
            #.#.#.......#.#
            #.#.#####.###.#
            #...........#.#
            ###.#.#####.#.#
            #...#.....#.#.#
            #.#.#.###.#.#.#
            #.....#...#.#.#
            #.###.#.#.#.#.#
            #S..#.....#...#
            ###############
            """));

        Assert.Equal(7036, result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day16(logger);

        var result = day.Solve(LinesFromSample(
            """
            #################
            #...#...#...#..E#
            #.#.#.#.#.#.#.#.#
            #.#.#.#...#...#.#
            #.#.#.#.###.#.#.#
            #...#.#.#.....#.#
            #.#.#.#.#.#####.#
            #.#...#.#.#.....#
            #.#.#####.#.###.#
            #.#.#.......#...#
            #.#.###.#####.###
            #.#.#...#.....#.#
            #.#.#.#####.###.#
            #.#.#.........#.#
            #.#.#.#########.#
            #S#.............#
            #################
            """));

        Assert.Equal(11048, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day16(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(85480, result);
    }

    [Fact]
    public void PartTwo_Sample_1()
    {
        var day = new Day16(logger);

        var result = day.SolveBonus(LinesFromSample(
                """
                ###############
                #.......#....E#
                #.#.###.#.###.#
                #.....#.#...#.#
                #.###.#####.#.#
                #.#.#.......#.#
                #.#.#####.###.#
                #...........#.#
                ###.#.#####.#.#
                #...#.....#.#.#
                #.#.#.###.#.#.#
                #.....#...#.#.#
                #.###.#.#.#.#.#
                #S..#.....#...#
                ###############
                """));

        Assert.Equal(45, result);
    }

    [Fact]
    public void PartTwo_Sample_2()
    {
        var day = new Day16(logger);

        var result = day.SolveBonus(LinesFromSample(
            """
            #################
            #...#...#...#..E#
            #.#.#.#.#.#.#.#.#
            #.#.#.#...#...#.#
            #.#.#.#.###.#.#.#
            #...#.#.#.....#.#
            #.#.#.#.#.#####.#
            #.#...#.#.#.....#
            #.#.#####.#.###.#
            #.#.#.......#...#
            #.#.###.#####.###
            #.#.#...#.....#.#
            #.#.#.#####.###.#
            #.#.#.........#.#
            #.#.#.#########.#
            #S#.............#
            #################
            """));

        Assert.Equal(64, result);
    }

    [Fact]
    public void PartTwo()
    {
        var day = new Day16(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(518, result);
    }
}
