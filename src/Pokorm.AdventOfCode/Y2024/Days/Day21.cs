namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/21
public class Day21(ILogger<Day21> logger)
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var keypadControl = new RobotKeypadControl(new NumericKeypad(), null);
        var dir1Control = new RobotKeypadControl(new DirectionalKeypad(), keypadControl);
        var dir2Control = new RobotKeypadControl(new DirectionalKeypad(), dir1Control);
        var dir3Control = new DirectKeypadControl(new DirectionalKeypad(), dir2Control);

        Run(data.Codes, dir3Control);

        var result = 0;

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
    }

    private static DayData Parse(string[] lines)
    {
        var codes = new List<KeypadCode>();

        foreach (var line in lines)
        {
            codes.Add(new (line));
        }

        return new DayData(codes);
    }

    private static void Run(List<KeypadCode> codes, KeypadControl control)
    {
        foreach (var c in codes)
        {
            Run(c, control);
        }
    }

    private static void Run(KeypadCode code, KeypadControl control)
    {
        foreach (var pos in code.GetPositions())
        {
            control.FindShortestSequence(pos);
        }
    }

    private abstract record KeypadControl(Keypad Keypad, KeypadControl? Controlling)
    {
        public abstract IEnumerable<KeypadPosition> FindShortestSequence(KeypadPosition pos);
    }

    private record DirectKeypadControl(Keypad Keypad, KeypadControl? Controlling) : KeypadControl(Keypad, Controlling)
    {
        public override IEnumerable<KeypadPosition> FindShortestSequence(KeypadPosition pos)
        {
            if (Controlling is not RobotKeypadControl r)
            {
                throw new Exception("cannot control direct");
            }

            if (Keypad is not DirectionalKeypad k)
            {
                throw new Exception("must control directional keypad");
            }
            
            var posValue = Keypad.Grid.Values[r.CurrentPosition]!;

            if (pos == posValue) // the subcontrol is already at the position
            {
                return [ new EnterPosition() ];
            }
            
            
        }
    }

    private record RobotKeypadControl(Keypad Keypad, KeypadControl? Controlling) : KeypadControl(Keypad, Controlling)
    {
        public Coord CurrentPosition { get; private set; } = Keypad.InitialPosition;

        public override IEnumerable<KeypadPosition> FindShortestSequence(KeypadPosition pos)
        {
            var posValue = Keypad.Grid.Values[CurrentPosition]!;

            if (pos == posValue)
            {
                yield break;
            }
            
            
        }
    }

    private abstract record KeypadPosition;

    private record EnterPosition : KeypadPosition;

    private record DirectionalPosition(Direction Direction) : KeypadPosition;

    private record NumberPosition(int Number) : KeypadPosition;

    private record Keypad(Grid<KeypadPosition?> Grid, Coord InitialPosition) { }

    private static Grid<KeypadPosition?> ParseKeypadGrid(string[] lines)
    {
        return Parser.ParseValuedGrid<KeypadPosition?>(lines, ToKeypadPosition);
    }

    static KeypadPosition? ToKeypadPosition(char c)
    {
        return c switch
        {
            '#'                        => null,
            '^' or '>' or '<' or 'v'   => new DirectionalPosition(c.ToDirection()),
            'A'                        => new EnterPosition(),
            var v when char.IsDigit(v) => new NumberPosition(int.Parse(v.ToString())),
            var _                      => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }

    private record DirectionalKeypad() : Keypad(ParseKeypadGrid([
        "#^A",
        "<v>"
    ]), new Coord(2, 0)) { }

    private record NumericKeypad() : Keypad(ParseKeypadGrid([
        "789",
        "456",
        "123",
        "#0A"
    ]), new Coord(2, 0)) { }

    private record KeypadCode(string RawForm)
    {
        private int? num;

        public int Number => this.num ??= int.Parse(new string(this.RawForm.Where(char.IsDigit).ToArray()));

        public IEnumerable<KeypadPosition> GetPositions()
        {
            foreach (var c in this.RawForm)
            {
                yield return ToKeypadPosition(c) ?? throw new Exception("bad code");
            }
        }
    }

    private record DayData(List<KeypadCode> Codes) { }
}
