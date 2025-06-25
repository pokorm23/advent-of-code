namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/21
public class Day21(ILogger<Day21> logger)
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var keypadControl = new RobotControl(new NumericKeypad());
        var dir1Control = new RobotControl(new DirectionalKeypad());

        var thre = "<v<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A";
        var m = thre.Select(ToKeypadPosition).OfType<KeypadPosition>().ToList();

        var a = dir1Control.Move(m).ToList();
        var b = dir1Control.Move(a).ToList();
        var c = keypadControl.Move(b).ToList();


        var result = 0L;

        foreach (var keypadCode in data.Codes)
        {
            var log = keypadCode.Number == 379;

            // 12
            var dirs1 = keypadControl.GetDirections(keypadCode.GetPositions().ToList()).SelectMany(x => x).ToList();

            // 28
            var dirs2 = dir1Control.GetDirections(dirs1).SelectMany(x => x).ToList();

            // 68
            var dirs3 = dir1Control.GetDirections(dirs2).SelectMany(x => x).ToList();

            if (log)
            {
                logger.LogInformation(string.Join("", m.Select(x => x.ToString())));
                logger.LogInformation(string.Join("", dirs3.Select(x => x.ToString())));
                logger.LogInformation("");

                logger.LogInformation(string.Join("", a.Select(x => x.ToString())));
                logger.LogInformation(string.Join("", dirs2.Select(x => x.ToString())));
                logger.LogInformation("");

                logger.LogInformation(string.Join("", b.Select(x => x.ToString())));
                logger.LogInformation(string.Join("", dirs1.Select(x => x.ToString())));
                logger.LogInformation("");

                logger.LogInformation("379A");
            }

            logger.LogInformation($"{dirs3.Count} * {keypadCode.Number}");

            result += dirs3.Count * (long) keypadCode.Number;
        }

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

    private record RobotState
    {
        private readonly Keypad keypad;

        public RobotState(Keypad keypad)
        {
            this.keypad = keypad;
            this.CurrentPosition = keypad.InitialPosition;
        }

        public KeypadPosition CurrentPosition { get; private set; }

        public void SetPosition(KeypadPosition pos) => this.CurrentPosition = pos;
    }

    private record RobotControl(Keypad Keypad)
    {
        public IEnumerable<KeypadPosition> Move(IReadOnlyCollection<KeypadPosition> input)
        {
            var p = this.Keypad.Grid.Values.SingleOrDefault(x => x.Value == this.Keypad.InitialPosition).Key;

            foreach (var pos in input)
            {
                if (pos is EnterPosition)
                {
                    yield return this.Keypad.Grid.Values.SingleOrDefault(x => x.Key == p).Value!;
                }
                else if (pos is DirectionalPosition {Direction: { } d})
                {
                    p = p + d.GetVector();
                }
            }
        }

        public IEnumerable<List<KeypadPosition>> GetDirections(IReadOnlyCollection<KeypadPosition> outcome)
        {
            var from = this.Keypad.InitialPosition;

            foreach (var pos in outcome)
            {
                var dirs = new List<KeypadPosition>();
                var to = pos;

                /*if (from is NumberPosition {Number: 3}
                    && to is NumberPosition {Number: 7})
                {
                    from = to;

                    yield return [
                        new DirectionalPosition(Direction.Left),
                        new DirectionalPosition(Direction.Left),
                        new DirectionalPosition(Direction.Top),
                        new DirectionalPosition(Direction.Top),
                        new EnterPosition() ];
                    continue;
                }*/

                var a = this.Keypad.Grid.Values.Single(x => x.Value == from).Key;
                var b = this.Keypad.Grid.Values.Single(x => x.Value == to).Key;
                var v = a - b;

                //var nullPos = this.keypad.Grid.Values.Single(x => x.Value is null).Key;

                var dir = v.ToDirection();

                if (dir is null)
                {
                    dirs.Add(new EnterPosition());

                    yield return dirs;

                    continue;
                }

                var ss = Permutations.GetPermutations([ Direction.Left, Direction.Right, Direction.Top, Direction.Bottom, ]).Select(x => x.ToList()).ToList();

                var p = this.Keypad is DirectionalKeypad
                                    ? ([ Direction.Bottom, Direction.Right, Direction.Top, Direction.Left ])
                                    : (from, to) switch
                                    {
                                        //(EnterPosition or NumberPosition { Number: 0}, _) => [ Direction.Top,Direction.Right,Direction.Bottom,   Direction.Left ],
                                        (NumberPosition {Number: > 0}, NumberPosition {Number: > 0}) => ss[23],
                                        var _                                                        => [ Direction.Top, Direction.Right, Direction.Bottom, Direction.Left ]
                                    };

                foreach (var d in p)
                {
                    var l = d is Direction.Left or Direction.Right ? v.X : v.Y;

                    if (dir.Value.HasFlag(d))
                    {
                        dirs.AddRange(Enumerable.Repeat(new DirectionalPosition(d), (int) Math.Abs(l)));
                    }
                }

                dirs.Add(new EnterPosition());

                from = to;

                yield return dirs;
            }
        }
    }

    private abstract record KeypadPosition;

    private record EnterPosition : KeypadPosition
    {
        public override string ToString() => "A";
    }

    private record DirectionalPosition(Direction Direction) : KeypadPosition
    {
        public override string ToString() => this.Direction.ToChar().ToString();
    }

    private record NumberPosition(int Number) : KeypadPosition
    {
        public override string ToString() => this.Number.ToString();
    }

    private record Keypad(Grid<KeypadPosition?> Grid, KeypadPosition InitialPosition)
    {
        public override string ToString() => this.Grid.ToString();
    }

    private static Grid<KeypadPosition?> ParseKeypadGrid(string[] lines) => Parser.ParseValuedGrid<KeypadPosition?>(lines, ToKeypadPosition);

    private static KeypadPosition? ToKeypadPosition(char c)
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
