using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day23Tests(ILogger<Day23> logger) : DayTestBase
{
    [Fact]
    public void PartOne_1()
    {
        var day = new Day23(logger);

        var result = day.Solve(LinesFromSample(
                """
                kh-tc
                qp-kh
                de-cg
                ka-co
                yn-aq
                qp-ub
                cg-tb
                vc-aq
                tb-ka
                wh-tc
                yn-cg
                kh-ub
                ta-co
                de-co
                tc-td
                tb-wq
                wh-td
                ta-ka
                td-qp
                aq-cg
                wq-ub
                ub-vc
                de-ta
                wq-aq
                wq-vc
                wh-yn
                ka-de
                kh-ta
                co-tc
                wh-qp
                tb-vc
                td-yn
                """));

        Assert.Equal(7, result);
    }

    [Fact]
    public void PartOne_F()
    {
        var day = new Day23(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal(1337, result);
    }
    
    [Fact]
    public void PartTwo_0()
    {
        var day = new Day23(logger);

        var result = day.SolveBonus(LinesFromSample(
            """
            a-b
            b-e
            a-c
            a-d
            c-d
            b-d
            b-c
            """));

        Assert.Equal("a,b,c,d", result);
    }

    [Fact]
    public void PartTwo_1()
    {
        var day = new Day23(logger);

        var result = day.SolveBonus(LinesFromSample(
                """
                kh-tc
                qp-kh
                de-cg
                ka-co
                yn-aq
                qp-ub
                cg-tb
                vc-aq
                tb-ka
                wh-tc
                yn-cg
                kh-ub
                ta-co
                de-co
                tc-td
                tb-wq
                wh-td
                ta-ka
                td-qp
                aq-cg
                wq-ub
                ub-vc
                de-ta
                wq-aq
                wq-vc
                wh-yn
                ka-de
                kh-ta
                co-tc
                wh-qp
                tb-vc
                td-yn
                """));

        Assert.Equal("co,de,ka,ta", result);
    }

    [Fact]
    public void PartTwo_F()
    {
        var day = new Day23(logger);

        var result = day.SolveBonus(LinesForDay(day));

        Assert.Equal("aw,fk,gv,hi,hp,ip,jy,kc,lk,og,pj,re,sr", result);
    }
}
