using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day20Tests(ILogger<Day20> logger) : DayTestBase
{
    [Fact]
    public void PartOne_1()
    {
        var day = new Day20(logger);

        var result = day.SolveCheats(LinesFromSample(
            """
            ###############
            #...#...#.....#
            #.#.#.#.#.###.#
            #S#...#.#.#...#
            #######.#.#.###
            #######.#.#...#
            #######.#.###.#
            ###..E#...#...#
            ###.#######.###
            #...###...#...#
            #.#####.#.###.#
            #.#...#.#.#...#
            #.#.#.#.#.#.###
            #...#...#...###
            ###############
            """), 2);

        Assert.Equal(14, result[2]);
        Assert.Equal(14, result[4]);
        Assert.Equal(2, result[6]);
        Assert.Equal(4, result[8]);
        Assert.Equal(2, result[10]);
        Assert.Equal(3, result[12]);
        Assert.Equal(1, result[20]);
        Assert.Equal(1, result[36]);
        Assert.Equal(1, result[38]);
        Assert.Equal(1, result[40]);
        Assert.Equal(1, result[64]);
    }

    [Fact]
    public void PartOne_F()
    {
        var day = new Day20(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(1497, result);
    }

    [Fact]
    public void PartTwo_1()
    {
        var day = new Day20(logger);

        var result = day.SolveCheats(LinesFromSample(
            """
            ###############
            #...#...#.....#
            #.#.#.#.#.###.#
            #S#...#.#.#...#
            #######.#.#.###
            #######.#.#...#
            #######.#.###.#
            ###..E#...#...#
            ###.#######.###
            #...###...#...#
            #.#####.#.###.#
            #.#...#.#.#...#
            #.#.#.#.#.#.###
            #...#...#...###
            ###############
            """), 20);

        Assert.Equal(32, result[50]);
        Assert.Equal(31, result[52]);
        Assert.Equal(29, result[54]);
        Assert.Equal(39, result[56]);
        Assert.Equal(25, result[58]);
        Assert.Equal(23, result[60]);
        Assert.Equal(20, result[62]);
        Assert.Equal(19, result[64]);
        Assert.Equal(12, result[66]);
        Assert.Equal(14, result[68]);
        Assert.Equal(12, result[70]);
        Assert.Equal(22, result[72]);
        Assert.Equal(4, result[74]);
        Assert.Equal(3, result[76]);
    }

    [Fact]
    public void PartTwo_F()
    {
        var day = new Day20(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(1030809, result);
    }
}
