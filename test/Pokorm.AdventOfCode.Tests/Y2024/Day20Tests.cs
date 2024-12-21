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
                """));

          Assert.Equal(14, result[2]);
          Assert.Equal(14, result[4]);
          Assert.Equal(2, result[5]);
          Assert.Equal(4, result[6]);
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

        Assert.Equal(-1, result);
    }

    /*[Fact]
    public void PartTwo_1()
    {
        var day = new Day20(logger);

        var result = day.SolveBonus(LinesFromSample(
                """
                ...
                """));

        Assert.Equal(-1, result);
    }

    [Fact]
    public void PartTwo_F()
    {
        var day = new Day20(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal(-1, result);
    }*/
}
