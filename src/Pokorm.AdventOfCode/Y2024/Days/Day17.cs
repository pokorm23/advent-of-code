using System.Text;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/17
public partial class Day17(ILogger<Day17> logger)
{
    public string Solve(string[] lines)
    {
        var data = Parse(lines);

        var computer = new Computer(new ComputerState(0, data.InitialRegisters with { }), Computer.Instructions);

        var sb = new StringBuilder();

        using (var s = new StringWriter(sb))
        {
            computer.Run(data.Program, computer.InitialState, s);
        }

        return sb.ToString();
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var i = 0u;
        var sb = new StringBuilder();

        for (; i < uint.MaxValue; i++)
        {
            var regs = data.InitialRegisters with
            {
                A = new Register('A')
                {
                    Value = i
                }
            };

            var computer = new Computer(new ComputerState(0, regs), Computer.Instructions);

            using (var s = new StringWriter(sb))
            {
                computer.Run(data.Program, computer.InitialState, s);
            }

            if (sb.ToString() == string.Join(",", data.Program.Select(x => x.Value)))
            {
                break;
            }

            sb.Clear();
        }

        return i;
    }

    #region Parsing

    [GeneratedRegex(@"Register (?<id>A|B|C): (?<val>\d+)")]
    public static partial Regex RegisterRegex { get; }

    [GeneratedRegex(@"Program: (?<val>[\d,]+)")]
    public static partial Regex ProgramRegex { get; }

    private static ProgramDefinition Parse(string[] lines)
    {
        var middle = lines.Index().FirstOrDefault(x => string.IsNullOrWhiteSpace(x.Item)).Index;

        var (regs, prog) = (lines[..middle], lines[(middle + 1)..]);

        var a = ParseRegister(regs[0]);
        var b = ParseRegister(regs[1]);
        var c = ParseRegister(regs[2]);

        var p = ProgramRegex.Match(prog[0])
                            .Groups["val"]
                            .Value
                            .Split(',')
                            .Select(uint.Parse)
                            .Select(x => (Bit3) x)
                            .ToList();

        return new ProgramDefinition(new ComputerRegisters(a, b, c), p);
    }

    private static Register ParseRegister(string value)
    {
        var match = RegisterRegex.Match(value);

        if (!match.Success)
        {
            throw new Exception("not a reg");
        }

        var id = match.Groups["id"].ValueSpan;
        var val = uint.Parse(match.Groups["val"].ValueSpan);

        return new Register(id[0])
        {
            Value = val
        };
    }

    private record ProgramDefinition(ComputerRegisters InitialRegisters, List<Bit3> Program) { }

    #endregion

    #region Instructions

    private abstract record DvIns(string Name, Bit3 OpCode) : Instruction(Name, OpCode)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            var num = ctx.RegisterA.Value;
            var den = (uint) Math.Pow(2, op.Combo);

            var div = Math.DivRem(num, den).Quotient;

            ctx.WriteRegister(this.RegisterSelector(ctx.CurrentState.Registers), div);
        }

        public abstract Func<ComputerRegisters, Register> RegisterSelector { get; }
    }

    private record AdvIns() : DvIns("adv", 0)
    {
        public override Func<ComputerRegisters, Register> RegisterSelector => r => r.A;
    }

    private record BdvIns() : DvIns("bdv", 6)
    {
        public override Func<ComputerRegisters, Register> RegisterSelector => r => r.B;
    }

    private record CdvIns() : DvIns("cdv", 7)
    {
        public override Func<ComputerRegisters, Register> RegisterSelector => r => r.C;
    }

    private record BxlIns() : Instruction("bxl", 1)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            ctx.WriteRegister(ctx.RegisterB, ctx.RegisterB.Value ^ op.Literal);
        }
    }

    private record BstIns() : Instruction("bst", 2)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            ctx.WriteRegister(ctx.RegisterB, op.ComboModulo);
        }
    }

    private record JnzIns() : Instruction("jnz", 3)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            if (ctx.RegisterA.Value == 0)
            {
                return;
            }

            ctx.SetInsPtr(op.Literal);
        }
    }

    private record BxcIns() : Instruction("bxc", 4)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            ctx.WriteRegister(ctx.RegisterB, ctx.RegisterB.Value ^ ctx.RegisterC.Value);
        }
    }

    private record OutIns() : Instruction("out", 5)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            var value = op.ComboModulo;

            ctx.WriteOutput(value);
        }
    }

    #endregion

    private abstract record Instruction(string Name, Bit3 OpCode)
    {
        public abstract void Run(Operand op, ComputerInstructionContext ctx);
    }

    private record Operand(Bit3 Value, ComputerRegisters Registers)
    {
        public Bit3 Literal => this.Value;

        public uint Combo => (uint) this.Value switch
        {
            >= 0 and <= 3 => this.Value,
            4             => this.Registers.A.Value,
            5             => this.Registers.B.Value,
            6             => this.Registers.C.Value,
            7             => throw new Exception("7 cannot be used in combo operand"),
            var _         => throw new ArgumentOutOfRangeException()
        };

        public Bit3 ComboModulo => this.Combo % 8;
    }

    private abstract record ComputerInstructionContext(ComputerState CurrentState)
    {
        public Register RegisterA => this.CurrentState.Registers.A;

        public Register RegisterB => this.CurrentState.Registers.B;

        public Register RegisterC => this.CurrentState.Registers.C;

        public abstract void SetInsPtr(uint newValue);

        public abstract void WriteOutput(uint value);

        public abstract void WriteRegister(Register reg, uint value);
    }

    private record ComputerControlContext(ComputerState CurrentState) : ComputerInstructionContext(CurrentState)
    {
        public uint? OverrideInsPtr { get; private set; }

        public Dictionary<Register, uint> RegisterWrites { get; } = [ ];

        public bool SuppressInsPtrIncrement => this.OverrideInsPtr.HasValue;

        public string? Output { get; set; }

        public override void SetInsPtr(uint newValue)
        {
            if (this.OverrideInsPtr.HasValue)
            {
                throw new Exception("ins ptr already set");
            }

            this.OverrideInsPtr = newValue;
        }

        public override void WriteOutput(uint value)
        {
            this.Output = value.ToString();
        }

        public override void WriteRegister(Register reg, uint value)
        {
            if (this.RegisterWrites.TryGetValue(reg, out var _))
            {
                throw new Exception($"{reg} already written");
            }

            this.RegisterWrites.Add(reg, value);
        }

        public Register GetFinalRegister(Register reg)
        {
            if (this.RegisterWrites.TryGetValue(reg, out var n))
            {
                return reg with
                {
                    Value = n
                };
            }

            return reg;
        }

        public ComputerRegisters GetFinalRegisters() => new ComputerRegisters(GetFinalRegister(this.CurrentState.Registers.A), GetFinalRegister(this.CurrentState.Registers.B), GetFinalRegister(this.CurrentState.Registers.C));
    }

    private record Computer(ComputerState InitialState, HashSet<Instruction> InstructionSet)
    {
        public static HashSet<Instruction> Instructions =
        [
            new AdvIns(),
            new BdvIns(),
            new BstIns(),
            new BxcIns(),
            new CdvIns(),
            new JnzIns(),
            new OutIns(),
            new BxlIns()
        ];

        public void Run(List<Bit3> program, ComputerState state, TextWriter stdOut)
        {
            var wasAnyStdOut = false;
            var runContext = new ComputerRunContext(program.ToArray(), state);

            while (!runContext.EndOfProgram)
            {
                var ins = ReadInstruction(runContext);

                var operand = ReadOperand(runContext);

                var controlContext = new ComputerControlContext(runContext.State with { });

                ins.Run(operand, controlContext);

                if (!controlContext.SuppressInsPtrIncrement)
                {
                    runContext.InsPtr++;
                }
                else if (controlContext.OverrideInsPtr is { } nip)
                {
                    runContext.InsPtr = nip;
                }
                else
                {
                    throw new Exception($"Invalid next ins pointer state");
                }

                if (!string.IsNullOrWhiteSpace(controlContext.Output))
                {
                    if (wasAnyStdOut)
                    {
                        stdOut.Write(',');
                    }

                    stdOut.Write(controlContext.Output);

                    wasAnyStdOut = true;
                }

                runContext = runContext with
                {
                    State = new ComputerState(runContext.InsPtr, controlContext.GetFinalRegisters())
                };
            }
        }

        private Instruction ReadInstruction(ComputerRunContext ctx)
        {
            var nextOpCode = ctx.Program[ctx.InsPtr];

            var ins = this.InstructionSet.FirstOrDefault(x => x.OpCode == nextOpCode);

            if (ins is null)
            {
                throw new Exception($"Invalid OpCode: {nextOpCode}");
            }

            ctx.InsPtr++;

            return ins;
        }

        private Operand ReadOperand(ComputerRunContext ctx)
        {
            if (ctx.InsPtr >= ctx.Program.Length)
            {
                throw new Exception($"Cannot read opcode due to end of program");
            }

            return new Operand(ctx.Program[ctx.InsPtr], ctx.State.Registers with { });
        }

        private record ComputerRunContext(Bit3[] Program, ComputerState State)
        {
            public uint InsPtr { get; set; }

            public bool EndOfProgram => this.InsPtr == this.Program.Length;

            public bool OutOfBound => this.InsPtr > this.Program.Length;
        }
    }

    private record ComputerState(uint InsPtr, ComputerRegisters Registers)
    {
        public override string ToString() => $"{this.Registers}{Environment.NewLine}InsPtr{this.InsPtr}";
    }

    private record Register(char Id)
    {
        public uint Value { get; init; }

        public override string ToString() => $"Register {this.Id}: {this.Value}";
    }

    private record ComputerRegisters(Register A, Register B, Register C)
    {
        public override string ToString() => $"{this.A}{Environment.NewLine}{this.B}{Environment.NewLine}{this.C}";
    }

    private record struct Bit3(bool Most, bool Middle, bool Least)
    {
        public static implicit operator uint(Bit3 bit) => (bit.Least ? 1 : 0u) + ((bit.Middle ? 1 : 0u) << 1) + ((bit.Most ? 1 : 0u) << 2);

        public static implicit operator Bit3(uint value)
        {
            if (value > 7)
            {
                throw new Exception("not a bit 3");
            }

            var most = (value & 0b100) >> 2 == 1;
            var middle = (value & 0b010) >> 1 == 1;
            var least = (value & 0b001) == 1;

            return new Bit3(most, middle, least);
        }

        public uint Value => (uint) this;
    }
}
