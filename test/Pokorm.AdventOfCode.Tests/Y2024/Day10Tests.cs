using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day10Tests : DayTestBase
{
    public Day10Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day10();

        var result = day.Solve(LinesFromSample(
            """
            0123
            1234
            8765
            9876
            """));

        Assert.Equal(1, result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day10();

        var result = day.Solve(LinesFromSample(
            """
            ...0...
            ...1...
            ...2...
            6543456
            7.....7
            8.....8
            9.....9
            """));

        Assert.Equal(2, result);
    }

    [Fact]
    public void PartOne_Sample_3()
    {
        var day = new Day10();

        var result = day.Solve(LinesFromSample(
            """
            ..90..9
            ...1.98
            ...2..7
            6543456
            765.987
            876....
            987....
            """));

        Assert.Equal(4, result);
    }

    [Fact]
    public void PartOne_Sample_4()
    {
        var day = new Day10();

        var result = day.Solve(LinesFromSample(
            """
            10..9..
            2...8..
            3...7..
            4567654
            ...8..3
            ...9..2
            .....01
            """));

        Assert.Equal(3, result);
    }

    [Fact]
    public void PartOne_Sample_5()
    {
        var day = new Day10();

        var result = day.Solve(LinesFromSample(
            """
            89010123
            78121874
            87430965
            96549874
            45678903
            32019012
            01329801
            10456732
            """));

        Assert.Equal(36, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day10();

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(754, result);
    }

    /* [Fact]
     public void PartTwo_Sample()
     {
         var day = new Day10();

         var result = day.SolveBonus(LinesFromSample(
                 """
                 ...
                 """));

         Assert.Equal(-1, result);
     }

     [Fact]
     public void PartTwo()
     {
         var day = new Day10();

         var result = day.SolveBonus(LinesForDay(day));

         Assert.Equal(-1, result);
     }*/
}
