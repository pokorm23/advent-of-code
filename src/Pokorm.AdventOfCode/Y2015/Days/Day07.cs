using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/7
public partial class Day07
{
    #region Types

    private abstract record Value
    {
        public ushort? CachedValue { get; protected set; }

        public ushort Evaluate()
        {
            if (CachedValue is not null)
            {
                return CachedValue.Value;
            }

            var result = EvaluateCore();

            CachedValue = result;

            return result;
        }

        protected abstract ushort EvaluateCore();
    }

    private record LShiftOp(Value A, Value B) : Value
    {
        protected override ushort EvaluateCore() => (ushort) (this.A.Evaluate() << this.B.Evaluate());
    }

    private record RShiftOp(Value A, Value B) : Value
    {
        protected override ushort EvaluateCore() => (ushort) (this.A.Evaluate() >> this.B.Evaluate());
    }

    private record NotOp(Value A) : Value
    {
        protected override ushort EvaluateCore() => (ushort) ~this.A.Evaluate();
    }

    private record BufferOp(Value A) : Value
    {
        protected override ushort EvaluateCore() => (ushort) this.A.Evaluate();
    }

    private record AndOp(AndOpDescriptor Descriptor, Value A, Value B) : Value
    {
        protected override ushort EvaluateCore() => (ushort) (this.A.Evaluate() & this.B.Evaluate());

        public override string ToString() => this.Descriptor.ToString();
    }

    private record OrOp(OrOpDescriptor Descriptor, Value A, Value B) : Value
    {
        protected override ushort EvaluateCore() => (ushort) (this.A.Evaluate() | this.B.Evaluate());

        public override string ToString() => this.Descriptor.ToString();
    }

    private record ConstantOp(ushort Value) : Value
    {
        protected override ushort EvaluateCore() => this.Value;
    }

    private abstract record OperationDescriptor(WireOrConstant[] Operands)
    {
        public IEnumerable<string> GetWireNames()
        {
            return this.Operands.OfType<Wire>().Select(x => x.Name);
        }

        public abstract Value Create(CreateContext lookup);
    }

    private abstract record WireOrConstant()
    {
        public static WireOrConstant Create(string s)
        {
            if (ushort.TryParse(s, out var value))
            {
                return new Constant(value);
            }

            return new Wire(s);
        }
    }

    private record Constant(ushort Value) : WireOrConstant
    {
        public static implicit operator ushort(Constant o) => o.Value;

        public static implicit operator Constant(ushort o) => new Constant(o);

        public override string ToString() => this.Value.ToString();
    }

    private record Wire(string Name) : WireOrConstant
    {
        public static implicit operator string(Wire o) => o.Name;

        public static implicit operator Wire(string o) => new Wire(o);

        public override string ToString() => this.Name;
    }

    private record LShiftOpDescriptor(WireOrConstant A, WireOrConstant B) : OperationDescriptor([ A, B ])
    {
        public override LShiftOp Create(CreateContext lookup) => new LShiftOp(lookup[this.A], lookup[this.B]);

        public override string ToString() => $"{this.A} RSHIFT {this.B}";
    }

    private record RShiftOpDescriptor(WireOrConstant A, WireOrConstant B) : OperationDescriptor([ A, B ])
    {
        public override RShiftOp Create(CreateContext lookup) => new RShiftOp(lookup[this.A], lookup[this.B]);

        public override string ToString() => $"{this.A} LSHIFT {this.B}";
    }

    private record NotOpDescriptor(WireOrConstant A) : OperationDescriptor([ A ])
    {
        public override NotOp Create(CreateContext lookup) => new NotOp(lookup[this.A]);

        public override string ToString() => $"NOT {this.A}";
    }

    private record AndOpDescriptor(WireOrConstant A, WireOrConstant B) : OperationDescriptor([ A, B ])
    {
        public override AndOp Create(CreateContext lookup) => new AndOp(this, lookup[this.A], lookup[this.B]);

        public override string ToString() => $"{this.A} AND {this.B}";
    }

    private record OrOpDescriptor(WireOrConstant A, WireOrConstant B) : OperationDescriptor([ A, B ])
    {
        public override OrOp Create(CreateContext lookup) => new OrOp(this, lookup[this.A], lookup[this.B]);

        public override string ToString() => $"{this.A} OR {this.B}";
    }

    private record ConstantOpDescriptor(WireOrConstant A) : OperationDescriptor([ A ])
    {
        public override Value Create(CreateContext lookup) => lookup[this.A];

        public override string ToString() => $"{this.A}";
    }

    #endregion


    [GeneratedRegex(@"((?<Left>\S+) (?<Op>AND|OR) (?<Right>\S+))|((?<Left>\S+) (?<Op>LSHIFT|RSHIFT) (?<Right>\S+))")]
    public static partial Regex TwoOperandRegex();

    private static DayData Parse(string[] lines)
    {
        var gates = new HashSet<Gate>();

        var regex = TwoOperandRegex();

        foreach (var (i, line) in lines.Index())
        {
            var split = line.FullSplit("->");

            OperationDescriptor descriptor;

            if (split[0].StartsWith("NOT "))
            {
                descriptor = new NotOpDescriptor(WireOrConstant.Create(split[0][4..]));
            }
            else if (regex.IsMatch(split[0]))
            {
                var match = regex.Match(split[0]);

                var op = match.Groups["Op"].Value;
                var left = WireOrConstant.Create(match.Groups["Left"].Value);
                var right = WireOrConstant.Create(match.Groups["Right"].Value);

                if (op == "AND")
                {
                    descriptor = new AndOpDescriptor(left, right);
                }
                else if (op == "OR")
                {
                    descriptor = new OrOpDescriptor(left, right);
                }
                else if (op == "RSHIFT")
                {
                    descriptor = new RShiftOpDescriptor(left, right);
                }
                else if (op == "LSHIFT")
                {
                    descriptor = new LShiftOpDescriptor(left, right);
                }
                else
                {
                    throw new Exception(i.ToString());
                }
            }
            else
            {
                descriptor = new ConstantOpDescriptor(WireOrConstant.Create(split[0]));
            }

            var resultWire = split[1];

            if (!gates.Add(new Gate(resultWire, descriptor)))
            {
                throw new Exception();
            }
        }

        Debug.Assert(gates.Count == lines.Length);

        return new DayData(gates);
    }

    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var gates = data.GetOrderedGates().ToList();

        Debug.Assert(gates.Count == data.Gates.Count);

        var context = new CreateContext();

        foreach (var gate in gates)
        {
            var value = gate.GateDesc.Create(context);

            context.Add(gate, value);
        }

        var result = context["a"].Evaluate();

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
    }

    private record CreateContext()
    {
        private Dictionary<string, Value> Lookup { get; } = [ ];

        public Value this[WireOrConstant index]
        {
            get
            {
                Console.Write($"Context lok: {index}");

                if (index is Constant c)
                {
                    return new ConstantOp(c.Value);
                }
                else if (index is Wire w)
                {
                    return this.Lookup[w];
                }

                throw new Exception();
            }
        }

        public Value this[Wire index] => this[index as WireOrConstant];

        public Value this[Constant index] => this[index as WireOrConstant];

        public void Add(Gate gate, Value value)
        {
            Console.WriteLine($"Context add: {gate}");

            this.Lookup.Add(gate.ResultWireName, value);
        }
    }

    private sealed record Gate(string ResultWireName, OperationDescriptor GateDesc)
    {
        public IEnumerable<string> GetOperandWireNames() => this.GateDesc.GetWireNames();

        public bool Equals(Gate? other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other != null && this.ResultWireName != other.ResultWireName;
        }

        public override int GetHashCode() => HashCode.Combine(this.ResultWireName);

        public override string ToString() => $"{this.GateDesc} -> {this.ResultWireName}";
    }

    private record DayData(HashSet<Gate> Gates)
    {
        public IEnumerable<Gate> GetOrderedGates()
        {
            var lookup = this.Gates
                             .ToDictionary(x => x, x =>
                             {
                                 return x.GetOperandWireNames().Select(z => this.Gates.Single(y => y.ResultWireName == z)).ToHashSet();
                             });

            /*foreach (var rule in this.Gates)
            {
                if (!lookup.TryGetValue(rule, out var _))
                {
                    lookup.Add(rule, [ ]);
                }
            }*/

            var result = TopologicalSort(lookup).Reverse();

            return result;
        }

        static void TopologicalSortUtil(Gate v,
            Dictionary<Gate, HashSet<Gate>> adj,
            HashSet<Gate> visited,
            Stack<Gate> stack)
        {
            visited.Add(v);

            foreach (var i in adj[v])
            {
                if (visited.Contains(i))
                {
                    continue;
                }

                TopologicalSortUtil(i, adj, visited, stack);
            }

            stack.Push(v);
        }

        static IEnumerable<Gate> TopologicalSort(Dictionary<Gate, HashSet<Gate>> adj)
        {
            var stack = new Stack<Gate>();
            var visited = new HashSet<Gate>();

            foreach (var i in adj.Keys)
            {
                if (visited.Contains(i))
                {
                    continue;
                }

                TopologicalSortUtil(i, adj, visited, stack);
            }

            return stack;
        }
    }
}
