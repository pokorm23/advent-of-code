using System.Text.RegularExpressions;

namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/6
public partial class Day06
{
    [GeneratedRegex(@"(?<Op>turn off|turn on|toggle) (?<x1>\d{1,3}),(?<y1>\d{1,3}) through (?<x2>\d{1,3}),(?<y2>\d{1,3})")]
    public static partial Regex AllRegex();

    private static DayData Parse(string[] lines)
    {
        var r = AllRegex();
        var ins = new List<Ins>();

        foreach (var line in lines)
        {
            var match = r.Match(line);

            var op = match.Groups["Op"].Value switch
            {
                "turn off" => Operation.TurnOff,
                "turn on"  => Operation.TurnOn,
                "toggle"   => Operation.Toggle,
                var _      => throw new Exception()
            };

            var range = new Range(new Coord(int.Parse(match.Groups["x1"].Value), int.Parse(match.Groups["y1"].Value)),
                new Coord(int.Parse(match.Groups["x2"].Value), int.Parse(match.Groups["y2"].Value)));

            ins.Add(new Ins(op, range));
        }

        return new DayData(ins);
    }

    public long Solve(string[] lines)
    {
        var data = Parse(lines);
        var state = new State([ ]);

        foreach (var dataInstruction in data.Instructions)
        {
            state = state.ApplyInstruction(dataInstruction);
        }

        return state.LitLights.Count;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);
        var state = new BonusState([ ]);

        foreach (var dataInstruction in data.Instructions)
        {
            state = state.ApplyInstruction(dataInstruction);
        }

        return state.BrightnessMap.Sum(x => x.Value);
    }

    private enum Operation
    {
        TurnOn,
        TurnOff,
        Toggle
    }

    private record struct Coord(int X, int Y);

    private record struct Range(Coord A, Coord B)
    {
        public IEnumerable<Coord> Enumerate()
        {
            var x0 = Math.Min(this.A.X, this.B.X);
            var x1 = Math.Max(this.A.X, this.B.X);
            var y0 = Math.Min(this.A.Y, this.B.Y);
            var y1 = Math.Max(this.A.Y, this.B.Y);

            for (var x = x0; x <= x1; x++)
            {
                for (var y = y0; y <= y1; y++)
                {
                    yield return new Coord(x, y);
                }
            }
        }
    }

    private record struct Ins(Operation Operation, Range Range);

    private record State(HashSet<Coord> LitLights)
    {
        public State ApplyInstruction(Ins ins)
        {
            var newCurrent = this.LitLights.ToHashSet();

            foreach (var coord in ins.Range.Enumerate())
            {
                var newState = ins.Operation switch
                {
                    Operation.TurnOn  => true,
                    Operation.TurnOff => false,
                    Operation.Toggle  => !this.LitLights.Contains(coord),
                    var _             => throw new Exception()
                };

                if (newState)
                {
                    newCurrent.Add(coord);
                }
                else
                {
                    newCurrent.Remove(coord);
                }
            }

            return new State(newCurrent);
        }
    }

    private record BonusState(Dictionary<Coord, int> BrightnessMap)
    {
        public BonusState ApplyInstruction(Ins ins)
        {
            var newBrightnessMap = this.BrightnessMap.ToDictionary();

            foreach (var coord in ins.Range.Enumerate())
            {
                var brightnessChange = ins.Operation switch
                {
                    Operation.TurnOn  => 1,
                    Operation.TurnOff => -1,
                    Operation.Toggle  => 2,
                    var _             => throw new Exception()
                };

                if (!newBrightnessMap.TryAdd(coord, Math.Max(0, brightnessChange)))
                {
                    newBrightnessMap[coord] = Math.Max(0, newBrightnessMap[coord] + brightnessChange);
                }
            }

            return new BonusState(newBrightnessMap);
        }
    }

    private record DayData(List<Ins> Instructions) { }
}
