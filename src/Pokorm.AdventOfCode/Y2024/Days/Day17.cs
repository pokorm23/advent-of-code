using System.Text;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/17
public partial class Day17(ILogger<Day17> logger)
{
    public string Solve(string[] lines)
    {
        var data = Parse(lines);

        var computer = new Computer(Computer.AllInstructions);

        var (_, output, _, _) = computer.Run(data.Program, new ComputerState(0, data.InitialRegisters));

        return string.Join(",", output);
    }

    private bool Solve(int programIndex, long a, ComputerProgram p, out long foundA)
    {
        if (programIndex < 0)
        {
            foundA = a;

            return true;
        }

        // operation on A registers happens only on lower bit3, then shifts as program progresses
        for (var i = 0; i < 0b_1000; i++)
        {
            var newLower = (Bit3) i;
            var concatedA = 8 * a + newLower;
            var output = p.Bits[programIndex..].Select(x => (int) x).ToList();

            if (!IsProgramCopy(concatedA, p, output))
            {
                continue;
            }

            var partialSolve = Solve(programIndex - 1, concatedA, p, out foundA);

            if (partialSolve)
            {
                return partialSolve;
            }
        }

        foundA = -1;

        return false;
    }

    public long SolveBonus_Recursive(string[] lines)
    {
        var data = Parse(lines);

        var n = data.Program.Bits.Count;

        if (Solve(n - 1, 0, data.Program, out var result))
        {
            return result;
        }

        return -1;
    }

    public long SolveBonus_Naive(string[] lines)
    {
        var data = Parse(lines);

        var programOutput = data.Program.Bits.Select(x => x.Value).ToList();

        var result = long.MaxValue;

        for (var i = 0L; i < long.MaxValue; i++)
        {
            // logger.LogDebug($"A = {i}");

            if (!IsProgramCopy(i, data.Program, programOutput))
            {
                continue;
            }

            result = i;

            break;
        }

        return result;
    }

    private bool IsProgramCopy(long a, ComputerProgram p, List<int> output)
    {
        var regs = ComputerRegisters.Empty;

        regs = regs with
        {
            A = ComputerRegisters.CreateA(a)
        };

        var initState = new ComputerState(0, regs);

        var c = new Computer(Computer.AllInstructions)
        {
            OnAfterOutputChanged = (partOut, ctx) =>
            {
                if (partOut.Count > output.Count)
                {
                    return;
                }

                if (!partOut.SequenceEqual(output[..partOut.Count]))
                {
                    ctx.Halt();
                }
            }
        };

        var r = c.Run(p, initState);

        return r.Finished && r.Output.SequenceEqual(output);
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
                            .Select(long.Parse)
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
        var val = long.Parse(match.Groups["val"].ValueSpan);

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
        public override void Run(EvaluatedOperand op, ComputerInstructionContext ctx)
        {
            var num = ctx.RegisterA.Value;
            var den = (long) Math.Pow(2, op.Combo);

            var div = Math.DivRem(num, den).Quotient;

            ctx.WriteRegister(this.RegisterSelector(ctx.CurrentState.Registers), div);
        }

        public override string Format(Operand? operand)
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
        public override void Run(EvaluatedOperand op, ComputerInstructionContext ctx)
        {
            ctx.WriteRegister(ctx.RegisterB, ctx.RegisterB.Value ^ op.Literal);
        }

        public override string Format(Operand? operand) => $"B <- B ^ {Operand.FormatOperand(false, operand)}";
    }

    public class BstIns() : Instruction("bst", 2)
    {
        public override void Run(EvaluatedOperand op, ComputerInstructionContext ctx)
        {
            ctx.WriteRegister(ctx.RegisterB, op.ComboModulo);
        }

        public override string Format(Operand? operand) => $"B <- {Operand.FormatOperand(true, operand)} % 8";
    }

    public class JnzIns() : Instruction("jnz", 3)
    {
        public override void Run(EvaluatedOperand op, ComputerInstructionContext ctx)
        {
            if (ctx.RegisterA.Value == 0)
            {
                return;
            }

            ctx.SetInsPtr(op.Literal);
        }

        public override string Format(Operand? operand) => $"InsPtr <- A == 0 ? InsPtr : {Operand.FormatOperand(false, operand)}";
    }

    public class BxcIns() : Instruction("bxc", 4)
    {
        public override void Run(EvaluatedOperand op, ComputerInstructionContext ctx)
        {
            ctx.WriteRegister(ctx.RegisterB, ctx.RegisterB.Value ^ ctx.RegisterC.Value);
        }

        public override string Format(Operand? operand) => $"B <- B ^ C";
    }

    public class OutIns() : Instruction("out", 5)
    {
        public override void Run(EvaluatedOperand op, ComputerInstructionContext ctx)
        {
            var value = op.ComboModulo;

            ctx.WriteOutput(value);
        }

        public override string Format(Operand? operand) => $"Out <- {Operand.FormatOperand(true, operand)} % 8";
    }

    #endregion

    public abstract class Instruction(string name, Bit3 opCode)
    {
        public Bit3 OpCode { get; } = opCode;

        public string Name { get; } = name;

        public abstract void Run(EvaluatedOperand op, ComputerInstructionContext ctx);

        public virtual string Format(Operand? operand) => this.Name;

        public override string ToString() => name;
    }

    public record Operand(Bit3 Value)
    {
        public static string GetComboFormat(Operand value) => (value.Value.Value, value) switch
        {
            (>= 0 and <= 3, var _)   => value.Value.BinaryFormat(),
            (4, EvaluatedOperand op) => op.Registers.A.Value.ToString(),
            (4, var _)               => "A",
            (5, EvaluatedOperand op) => op.Registers.B.Value.ToString(),
            (5, var _)               => "B",
            (6, EvaluatedOperand op) => op.Registers.C.Value.ToString(),
            (6, var _)               => "C",
            (7, var _)               => throw new Exception("7 cannot be used in combo operand"),
            var _                    => throw new ArgumentOutOfRangeException()
        };

        public static string FormatOperand(bool isCombo, Operand? operand)
        {
            if (operand is not null)
            {
                return isCombo ? GetComboFormat(operand) : operand.Value.BinaryFormat();
            }

            return isCombo ? "Op(C)" : "Op(L)";
        }
    }

    public record EvaluatedOperand(Bit3 Value, ComputerRegisters Registers) : Operand(Value)
    {
        public Bit3 Literal => this.Value;

        public long Combo => (long) this.Value switch
        {
            >= 0 and <= 3 => this.Value,
            4             => this.Registers.A.Value,
            5             => this.Registers.B.Value,
            6             => this.Registers.C.Value,
            7             => throw new Exception("7 cannot be used in combo operand"),
            var _         => throw new ArgumentOutOfRangeException()
        };

        public Bit3 ComboModulo => (int) (this.Combo % 8);
    }

    public abstract record ComputerInstructionContext(ComputerState CurrentState)
    {
        public Register RegisterA => this.CurrentState.Registers.A;

        public Register RegisterB => this.CurrentState.Registers.B;

        public Register RegisterC => this.CurrentState.Registers.C;

        public abstract void SetInsPtr(int newValue);

        public abstract void WriteOutput(int value);

        public abstract void WriteRegister(Register reg, long value);
    }

    public record ComputerControlContext(ComputerState CurrentState) : ComputerInstructionContext(CurrentState)
    {
        public int? OverrideInsPtr { get; private set; }

        public Dictionary<Register, long> RegisterWrites { get; } = [ ];

        public bool SuppressInsPtrIncrement => this.OverrideInsPtr.HasValue;

        public List<int> Output { get; set; } = new ();

        public override void SetInsPtr(int newValue)
        {
            if (this.OverrideInsPtr.HasValue)
            {
                throw new Exception("ins ptr already set");
            }

            this.OverrideInsPtr = newValue;
        }

        public override void WriteOutput(int value)
        {
            this.Output.Add(value);
        }

        public override void WriteRegister(Register reg, long value)
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

        public ComputerRegisters GetFinalRegisters() => new (GetFinalRegister(this.CurrentState.Registers.A), GetFinalRegister(this.CurrentState.Registers.B), GetFinalRegister(this.CurrentState.Registers.C));
    }

    public record RunInstruction(Instruction Ins, Operand Operand)
    {
        public string Format() => this.Ins.Format(this.Operand);

        public override string ToString() => $"{this.Ins}({this.Operand})";
    }

    public record ComputerProgram(List<Bit3> Bits, List<RunInstruction> Instructions)
    {
        public static ComputerProgram Parse(List<Bit3> bits, HashSet<Instruction> instructionSet)
        {
            var r = new List<RunInstruction>();

            foreach (var b in bits.Chunk(2))
            {
                var (ins, op) = (b[0], b[1]);

                r.Add(new RunInstruction(instructionSet.SingleOrDefault(x => x.OpCode == ins) ?? throw new Exception($"Invalid OpCode: {ins}"), new Operand(op)));
            }

            return new (bits, r);
        }

        public RunInstruction this[int ip] => this.Instructions[ip / 2];

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

    public record ComputerProgramRunResult(bool Finished, List<int> Output, ComputerState State, List<string> Log)
    {
        public IEnumerable<string> GetToStringLines()
        {
            yield return $"Finished = {this.Finished}";
            yield return $"Output   = {string.Join(",", this.Output)}";
            yield return $"State";

            foreach (var l in this.State.GetToStringLines())
            {
                yield return $" - {l}";
            }

            yield return $"Log";

            if (this.Log.Count == 0)
            {
                yield return $" - (none)";

                yield break;
            }

            foreach (var se in this.Log)
            {
                yield return $"  {se}";
            }
        }

        public override string ToString() => string.Join(Environment.NewLine, GetToStringLines());
    }

    public record Computer(HashSet<Instruction> InstructionSet)
    {
        public bool Log { get; set; }

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

        public Action<List<int>, ComputerRunContext>? OnAfterOutputChanged { get; set; }

        public ComputerProgramRunResult Run(ComputerProgram program, ComputerState state)
        {
            var output = new List<int>();
            var runContext = new ComputerRunContext(program, state);
            var log = this.Log ? null : new List<string>();
            var i = 0;

            while (!runContext.EndOfProgram)
            {
                if (runContext.HaltRequested)
                {
                    log?.Add($" - Computer halt request");

                    return new ComputerProgramRunResult(false, output, runContext.State, log);
                }

                var (ins, operandValue) = program[runContext.InsPtr];

                var operand = new EvaluatedOperand(operandValue.Value, runContext.State.Registers);

                var controlContext = new ComputerControlContext(runContext.State with { });

                log?.Add($"[{i + 1}] Running: {ins.Format(operand)}");

                ins.Run(operand, controlContext);

                if (!controlContext.SuppressInsPtrIncrement)
                {
                    runContext.InsPtr += 2;
                }
                else if (controlContext.OverrideInsPtr is { } nip)
                {
                    log?.Add($" - longPtr set: {nip}");
                    runContext.InsPtr = nip;
                }
                else
                {
                    throw new Exception($"Invalid next ins polonger state");
                }

                if (controlContext.Output.Count > 0)
                {
                    log?.Add($" - Output set: {string.Join(",", controlContext.Output)}");

                    output.AddRange(controlContext.Output);

                    this.OnAfterOutputChanged?.Invoke(output, runContext);
                }

                if (log is not null)
                {
                    foreach (var (r, v) in controlContext.RegisterWrites.OrderBy(x => x.Key.Id))
                    {
                        log?.Add($" - {r.Id}: {v}");
                    }
                }

                runContext = runContext with
                {
                    State = new ComputerState(runContext.InsPtr, controlContext.GetFinalRegisters())
                };

                i++;
            }

            return new ComputerProgramRunResult(true, output, runContext.State, log ?? [ ]);
        }

        public record ComputerRunContext(ComputerProgram Program, ComputerState State)
        {
            public int InsPtr { get; set; }

            public bool EndOfProgram => this.InsPtr == this.Program.RawLength;

            public bool OutOfBound => this.InsPtr > this.Program.RawLength;

            public bool HaltRequested { get; private set; }

            public void Halt()
            {
                this.HaltRequested = true;
            }
        }
    }

    public record ComputerState(long InsPtr, ComputerRegisters Registers)
    {
        public static ComputerState Empty = new (0, ComputerRegisters.Empty);

        public IEnumerable<string> GetToStringLines()
        {
            foreach (var l in this.Registers.GetToStringLines())
            {
                yield return l;
            }

            yield return $"InsPtr    : {this.InsPtr}";
        }

        public override string ToString() => string.Join(Environment.NewLine, GetToStringLines());
    }

    public record Register(char Id)
    {
        public long Value { get; init; }

        public override string ToString() => $"Register {this.Id}: {this.Value}";
    }

    public record ComputerRegisters(Register A, Register B, Register C)
    {
        public static Register CreateA(long v = 0) => new ('A')
        {
            Value = v
        };

        public static Register CreateB(long v = 0) => new ('B')
        {
            Value = v
        };

        public static Register CreateC(long v = 0) => new ('C')
        {
            Value = v
        };

        public static ComputerRegisters Empty => new (CreateA(), CreateB(), CreateC());

        public IEnumerable<string> GetToStringLines()
        {
            yield return this.A.ToString();
            yield return this.B.ToString();
            yield return this.C.ToString();
        }

        public override string ToString() => string.Join(Environment.NewLine, GetToStringLines());
    }

    public record struct Bit3(bool Most, bool Middle, bool Least)
    {
        public static implicit operator int(Bit3 bit) => (bit.Least ? 1 : 0) + ((bit.Middle ? 1 : 0) << 1) + ((bit.Most ? 1 : 0) << 2);

        public static implicit operator Bit3(int value)
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

        public int Value => (int) this;

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
