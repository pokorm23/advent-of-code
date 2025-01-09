namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/21
public class Day21(ILogger<Day21> logger)
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var keypadControl = new RobotControl(new NumericKeypad());
        var dir1Control = new RobotControl(new DirectionalKeypad());
        var dir2Control = new RobotControl(new DirectionalKeypad());
        var dir3Control = new RobotControl(new DirectionalKeypad());
        
        foreach (var keypadCode in data.Codes)
        {
            var lp = keypadControl.Keypad.InitialPosition;
            
            foreach (var pos in keypadCode.GetPositions())
            {
                keypadControl.MoveToPosition(lp, pos);

                lp = pos;
            }
        }

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

    record RobotControl(Keypad Keypad)
    {
        public int MoveToPosition(KeypadPosition from, KeypadPosition to)
        {
            var a = Keypad.Grid.Values.Single(x => x.Value == from).Key;
            var b = Keypad.Grid.Values.Single(x => x.Value == to).Key;

            var v = b - a;

            return (int) (v.X + v.Y);
        }
    }

    private abstract record KeypadPosition;

    private record EnterPosition : KeypadPosition;

    private record DirectionalPosition(Direction Direction) : KeypadPosition;

    private record NumberPosition(int Number) : KeypadPosition;

    private record Keypad(Grid<KeypadPosition?> Grid, KeypadPosition InitialPosition) { }

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
    ]), new EnterPosition()) { }

    private record NumericKeypad() : Keypad(ParseKeypadGrid([
        "789",
        "456",
        "123",
        "#0A"
    ]), new EnterPosition()) { }

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
