using System.Collections.Concurrent;
using System.Text;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/17
public partial class Day17(ILogger<Day17> logger)
{
    public string Solve(string[] lines)
    {
        var data = Parse(lines);

        var computer = new Computer(new ComputerState(0, data.InitialRegisters with { }), Computer.AllInstructions);

        var output = computer.Run(data.Program, computer.InitialState) ?? [ ];

        return string.Join(",", output);
    }

    public long SolveBonus(string[] lines)
    {
        return SolveBonusAcc(lines, null);
    }

    public long SolveBonusAcc(string[] lines, HashSet<Instruction>? acc)
    {
        var data = Parse(lines);

        var programOutput = data.Program.Bits.Select(x => x.Value).ToList();

        var res = new ConcurrentBag<int>();

        Parallel.For(0, 200_000, (i, ctx) =>
        {
            //logger.LogDebug($"A = {i}");

            var regs = data.InitialRegisters with
            {
                A = ComputerRegisters.CreateA((uint) i)
            };

            var computer = new Computer(new ComputerState(0, regs), acc ?? Computer.AllInstructions)
            {
                OnAfterOutputChanged = (partOut, ctx) =>
                {
                    if (partOut.Count > programOutput.Count)
                    {
                        return;
                    }

                    if (!partOut.SequenceEqual(programOutput[..partOut.Count]))
                    {
                        ctx.Halt();
                    }
                }
            };

            var output = computer.Run(data.Program, computer.InitialState);

            if (output is null || !output.SequenceEqual(programOutput))
            {
                return;
            }

            res.Add(i);
        });

        logger.LogDebug($"As = {string.Join(", ", res.Order())}");

        return res.Min();
    }

    #region Parsing

    [GeneratedRegex(@"Register (?<id>A|B|C): (?<val>\d+)")]
    public static partial Regex RegisterRegex { get; }

    [GeneratedRegex(@"Program: (?<val>[\d,]+)")]
    public static partial Regex ProgramRegex { get; }

    public static ProgramDefinition Parse(string[] lines)
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

        return new ProgramDefinition(new ComputerRegisters(a, b, c), ComputerProgram.Parse(p, Computer.AllInstructions));
    }

    public static Register ParseRegister(string value)
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

    public record ProgramDefinition(ComputerRegisters InitialRegisters, ComputerProgram Program) { }

    #endregion

    #region Instructions

    public abstract class DvIns(string name, Bit3 opCode) : Instruction(name, opCode)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            var num = ctx.RegisterA.Value;
            var den = 1u << (int) op.Combo;

            var div = Math.DivRem(num, den).Quotient;

            ctx.WriteRegister(this.RegisterSelector(ctx.CurrentState.Registers), div);
        }

        public override string Format(Bit3? operand)
        {
            var regName = this.RegisterSelector(ComputerRegisters.Empty).Id;

            return $"{regName} <- A // (1 << {Operand.FormatOperand(true, operand)})";
        }

        public abstract Func<ComputerRegisters, Register> RegisterSelector { get; }
    }

    public class AdvIns() : DvIns("adv", 0)
    {
        public override Func<ComputerRegisters, Register> RegisterSelector => r => r.A;
    }

    public class BdvIns() : DvIns("bdv", 6)
    {
        public override Func<ComputerRegisters, Register> RegisterSelector => r => r.B;
    }

    public class CdvIns() : DvIns("cdv", 7)
    {
        public override Func<ComputerRegisters, Register> RegisterSelector => r => r.C;
    }

    public class BxlIns() : Instruction("bxl", 1)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            ctx.WriteRegister(ctx.RegisterB, ctx.RegisterB.Value ^ op.Literal);
        }

        public override string Format(Bit3? operand) => $"B <- B ^ {Operand.FormatOperand(false, operand)}";
    }

    public class BstIns() : Instruction("bst", 2)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            ctx.WriteRegister(ctx.RegisterB, op.ComboModulo);
        }

        public override string Format(Bit3? operand) => $"B <- {Operand.FormatOperand(true, operand)} % 8";
    }

    public class JnzIns() : Instruction("jnz", 3)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            if (ctx.RegisterA.Value == 0)
            {
                return;
            }

            ctx.SetInsPtr(op.Literal);
        }

        public override string Format(Bit3? operand) => $"InsPtr <- A == 0 ? InsPtr : {Operand.FormatOperand(false, operand)}";
    }

    public class BxcIns() : Instruction("bxc", 4)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            ctx.WriteRegister(ctx.RegisterB, ctx.RegisterB.Value ^ ctx.RegisterC.Value);
        }

        public override string Format(Bit3? operand) => $"B <- B ^ C";
    }

    public class OutIns() : Instruction("out", 5)
    {
        public override void Run(Operand op, ComputerInstructionContext ctx)
        {
            var value = op.ComboModulo;

            ctx.WriteOutput(value);
        }

        public override string Format(Bit3? operand) => $"Out <- {Operand.FormatOperand(true, operand)} % 8";
    }



    #endregion

    public abstract class Instruction(string name, Bit3 opCode)
    {
        public Bit3 OpCode { get; } = opCode;

        public string Name { get; } = name;

        public abstract void Run(Operand op, ComputerInstructionContext ctx);

        public virtual string Format(Bit3? operand) => this.Name;

        public override string ToString() => Format(null);
    }

    public record Operand(Bit3 Value, ComputerRegisters Registers)
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

        public static string GetComboFormat(Bit3 value) => value.Value switch
        {
            >= 0 and <= 3 => value.BinaryFormat(),
            4             => "A",
            5             => "B",
            6             => "C",
            7             => throw new Exception("7 cannot be used in combo operand"),
            var _         => throw new ArgumentOutOfRangeException()
        };

        public Bit3 ComboModulo => this.Combo % 8;

        public static string FormatOperand(bool isCombo, Bit3? operand)
        {
            if (operand.HasValue)
            {
                return isCombo ? GetComboFormat(operand.Value) : operand.Value.BinaryFormat();
            }

            return isCombo ? "Op(C)" : "Op(L)";
        }
    }

    public abstract record ComputerInstructionContext(ComputerState CurrentState)
    {
        public Register RegisterA => this.CurrentState.Registers.A;

        public Register RegisterB => this.CurrentState.Registers.B;

        public Register RegisterC => this.CurrentState.Registers.C;

        public abstract void SetInsPtr(uint newValue);

        public abstract void WriteOutput(uint value);

        public abstract void WriteRegister(Register reg, uint value);
    }

    public record ComputerControlContext(ComputerState CurrentState) : ComputerInstructionContext(CurrentState)
    {
        public uint? OverrideInsPtr { get; private set; }

        public Dictionary<Register, uint> RegisterWrites { get; } = [ ];

        public bool SuppressInsPtrIncrement => this.OverrideInsPtr.HasValue;

        public List<uint> Output { get; set; } = new List<uint>();

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
            this.Output.Add(value);
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

    public record RunInstruction(Instruction Ins, Bit3 OperandValue)
    {
        public string Format() => this.Ins.Format(this.OperandValue);

        public override string ToString() => $"{this.Ins}({this.OperandValue})";
    }

    public record ComputerProgram(List<Bit3> Bits, List<RunInstruction> Instructions)
    {
        public static ComputerProgram Parse(List<Bit3> bits, HashSet<Instruction> instructionSet)
        {
            var r = new List<RunInstruction>();

            foreach (var b in bits.Chunk(2))
            {
                var (ins, op) = (b[0], b[1]);

                r.Add(new RunInstruction(instructionSet.SingleOrDefault(x => x.OpCode == ins) ?? throw new Exception($"Invalid OpCode: {ins}")
                  , op));
            }

            return new (bits, r);
        }

        public RunInstruction this[uint ip] => this.Instructions[(int) (ip / 2)];

        public int RawLength => this.Instructions.Count * 2;

        public string RawFormat => string.Join(",", this.Bits);

        public IEnumerable<string> GetFormatLines()
        {
            foreach (var i in this.Instructions)
            {
                yield return i.Format();
            }
        }

        public override string ToString() => $"{string.Join(",", this.Instructions)}";
    }

    public record Computer(ComputerState InitialState, HashSet<Instruction> InstructionSet)
    {
        public static HashSet<Instruction> AllInstructions =
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

        public Action<List<uint>, ComputerRunContext>? OnAfterOutputChanged { get; set; }

        public List<uint>? Run(ComputerProgram program, ComputerState state)
        {
            var output = new List<uint>();
            var runContext = new ComputerRunContext(program, state);

            while (!runContext.EndOfProgram)
            {
                if (runContext.HaltRequested)
                {
                    return null;
                }

                var (ins, operandValue) = program[runContext.InsPtr];

                var operand = new Operand(operandValue, runContext.State.Registers);

                var controlContext = new ComputerControlContext(runContext.State with { });

                ins.Run(operand, controlContext);

                if (!controlContext.SuppressInsPtrIncrement)
                {
                    runContext.InsPtr += 2;
                }
                else if (controlContext.OverrideInsPtr is { } nip)
                {
                    runContext.InsPtr = nip;
                }
                else
                {
                    throw new Exception($"Invalid next ins pointer state");
                }

                if (controlContext.Output.Count > 0)
                {
                    output.AddRange(controlContext.Output);

                    this.OnAfterOutputChanged?.Invoke(output, runContext);
                }

                runContext = runContext with
                {
                    State = new ComputerState(runContext.InsPtr, controlContext.GetFinalRegisters())
                };
            }

            return output;
        }

        public record ComputerRunContext(ComputerProgram Program, ComputerState State)
        {
            public uint InsPtr { get; set; }

            public bool EndOfProgram => this.InsPtr == this.Program.RawLength;

            public bool OutOfBound => this.InsPtr > this.Program.RawLength;

            public bool HaltRequested { get; private set; }

            public void Halt()
            {
                this.HaltRequested = true;
            }
        }
    }

    public record ComputerState(uint InsPtr, ComputerRegisters Registers)
    {
        public static ComputerState Empty = new ComputerState(0, ComputerRegisters.Empty);

        public override string ToString() => $"{this.Registers}{Environment.NewLine}InsPtr{this.InsPtr}";
    }

    public record Register(char Id)
    {
        public uint Value { get; init; }

        public override string ToString() => $"Register {this.Id}: {this.Value}";
    }

    public record ComputerRegisters(Register A, Register B, Register C)
    {
        public static Register CreateA(uint v = 0) => new Register('A')
        {
            Value = v
        };

        public static Register CreateB(uint v = 0) => new Register('B')
        {
            Value = v
        };

        public static Register CreateC(uint v = 0) => new Register('C')
        {
            Value = v
        };

        public static ComputerRegisters Empty => new ComputerRegisters(CreateA(), CreateB(), CreateC());

        public override string ToString() => $"{this.A}{Environment.NewLine}{this.B}{Environment.NewLine}{this.C}";
    }

    public record struct Bit3(bool Most, bool Middle, bool Least)
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

        public string BinaryFormat()
        {
            var sb = new StringBuilder();

            sb.Append(this.Most ? '1' : '0');
            sb.Append(this.Middle ? '1' : '0');
            sb.Append(this.Least ? '1' : '0');

            return sb.ToString();
        }

        public override string ToString() => this.Value.ToString();
    }
}
