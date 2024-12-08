using Pokorm.AdventOfCode.Y2015.Days;

namespace Pokorm.AdventOfCode.Tests.Y2015;

public class Day12Tests : DayTestBase
{
    public Day12Tests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void PartOne_Parse()
    {
        var day = new Day12();

        Assert.True(Day12.ParseJson("1") is Day12.JInteger {Value: 1});

        Assert.True(Day12.ParseJson("""
                                    "aaa "
                                    """) is Day12.JString {Value: "aaa "});

        Assert.True(Day12.ParseJson("[1,2,3]") is Day12.JArray
        {
            Items:
            [
                Day12.JInteger {Value: 1},
                Day12.JInteger {Value: 2},
                Day12.JInteger {Value: 3}
            ]
        });

        Assert.True(Day12.ParseJson("""
                                    {"a":1, "b":2}
                                    """) is Day12.JObject
        {
            Properties: { } p
        } && p.OrderBy(x => x.Key).ToList() is
        [
            {Key: "a", Value: Day12.JInteger {Value: 1}},
            {Key: "b", Value: Day12.JInteger {Value: 2}}
        ]);
    }

    [Fact]
    public void PartOne_Sample_1()
    {
        var day = new Day12();

        var result = day.Solve("""
                               [1,2,3]
                               """);

        Assert.Equal(6, result);
    }

    [Fact]
    public void PartOne_Sample_2()
    {
        var day = new Day12();

        var result = day.Solve("""
                               {"a":2,"b":4}
                               """);

        Assert.Equal(6, result);
    }

    [Fact]
    public void PartOne_Sample_3()
    {
        var day = new Day12();

        var result = day.Solve("""
                               [[[3]]]
                               """);

        Assert.Equal(3, result);
    }

    [Fact]
    public void PartOne_Sample_4()
    {
        var day = new Day12();

        var result = day.Solve("""
                               {"a":{"b":4},"c":-1}
                               """);

        Assert.Equal(3, result);
    }

    [Fact]
    public void PartOne_Sample_5()
    {
        var day = new Day12();

        var result = day.Solve("""
                               {"a":[-1,1]}
                               """);

        Assert.Equal(0, result);
    }

    [Fact]
    public void PartOne_Sample_6()
    {
        var day = new Day12();

        var result = day.Solve("""
                               [-1,{"a":1}]
                               """);

        Assert.Equal(0, result);
    }

    [Fact]
    public void PartOne()
    {
        var day = new Day12();

        var result = day.Solve(TextForDay(day));

        Assert.Equal(119433, result);
    }

    /* [Fact]
     public void PartTwo_Sample()
     {
         var day = new Day12();

         var result = day.SolveBonus(LinesFromSample(
                 """
                 ...
                 """));

         Assert.Equal(-1, result);
     }

     [Fact]
     public void PartTwo()
     {
         var day = new Day12();

         var result = day.SolveBonus(LinesForDay(day));

         Assert.Equal(-1, result);
     }*/
}
