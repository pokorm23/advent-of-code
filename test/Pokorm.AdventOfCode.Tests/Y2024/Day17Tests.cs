using Pokorm.AdventOfCode.Y2024.Days;

namespace Pokorm.AdventOfCode.Tests.Y2024;

public class Day17Tests(ILogger<Day17> logger) : DayTestBase
{
    [Fact]
    public void PartOne_1()
    {
        var day = new Day17(logger);

        var result = day.Solve(LinesFromSample(
            """
            Register A: 729
            Register B: 0
            Register C: 0

            Program: 0,1,5,4,3,0
            """));

        Assert.Equal("4,6,3,5,6,3,5,2,1,0", result);
    }

    [Fact]
    public void PartOne_F()
    {
        var day = new Day17(logger);

        var result = day.Solve(LinesForDay(day));

        Assert.Equal("1,6,7,4,3,0,5,0,6", result);
    }

    [Fact]
    public void PartTwo_1()
    {
        var day = new Day17(logger);

        var result = day.SolveBonus(LinesFromSample(
            """
            Register A: 2024
            Register B: 0
            Register C: 0

            Program: 0,3,5,4,3,0
            """));

        Assert.Equal(117440, result);
    }

    [Fact]
    public void PartTwo_F_1()
    {
        var day = new Day17(logger);

        var parse = Day17.Parse(LinesFromSample(
            """
            Register A: 63687530
            Register B: 0
            Register C: 0

            Program: 2,4,1,3,7,5,0,3,1,5,4,1,5,5,3,0
            """));

        logger.LogInformation(string.Join(Environment.NewLine, parse.Program.GetFormatLines()));

        var computer = new Day17.Computer(Day17.ComputerState.Empty, Day17.Computer.AllInstructions);
    }

    [Fact]
    public void PartTwo_F()
    {
        var day = new Day17(logger);

        //var result = day.SolveBonus(LinesForDay(day));
        var result = day.SolveBonusAcc(LinesFromSample(
            """
            Register A: 0
            Register B: 0
            Register C: 0

            Program: 0,0,1,0,2,0
            """), [ new B1Ins(), new B2Ins(), new B3Ins() ]);

        // Ab <- A % 8
        // A <- A // 1000
        // Out <- ((Ab ^ 101) ^ (A // (1 << Ab))) % 8
        // InsPtr <- A == 0 ? 0 : InsPtr

        Assert.Equal(-1, result);
    }

    public class B1Ins() : Day17.Instruction("b1", 0)
    {
        public override void Run(Day17.Operand op, Day17.ComputerInstructionContext ctx)
        {
            ctx.WriteRegister(ctx.RegisterA, ctx.RegisterA.Value / 0b1000);
        }

        public override string Format(Day17.Bit3? operand) => $"A <- A // 1000";
    }
    public class B2Ins() : Day17.Instruction("b2", 1)
    {
        public override void Run(Day17.Operand op, Day17.ComputerInstructionContext ctx)
        {
            var a = (int)ctx.RegisterA.Value;
            var ab = (int)ctx.RegisterA.Value % 8;

            ctx.WriteOutput((uint) ((ab ^ 0b101 ^ (a / (1 << ab))) % 8));
        }

        public override string Format(Day17.Bit3? operand) => $"Out <- ((A % 8 ^ 101) ^ (A // (1 << A % 8))) % 8";
    }
    public class B3Ins() : Day17.Instruction("b2", 2)
    {
        public override void Run(Day17.Operand op, Day17.ComputerInstructionContext ctx)
        {
            if (ctx.RegisterA.Value == 0)
            {
                return;
            }

            ctx.SetInsPtr(0);
        }

        public override string Format(Day17.Bit3? operand) => $"InsPtr <- A == 0 ? InsPtr : 0";
    }
}
