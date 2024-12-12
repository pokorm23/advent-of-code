using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/14
public partial class Day14
{
    private readonly ILogger<Day14> logger;

    [GeneratedRegex(@"(?<name>\w+) can fly (?<speed>\d+) km/s for (?<flyTime>\d+) seconds, but then must rest for (?<restTime>\d+) seconds\.")]
    public static partial Regex LineRegex { get; }

    public Day14(ILogger<Day14> logger) => this.logger = logger;

    public long Solve(string[] lines) => SolveIterations(lines, 2503);

    public long SolveBonus(string[] lines) => SolveIterations(lines, 0);

    public long SolveIterations(string[] lines, int iterations)
    {
        var data = Parse(lines);

        var state = new State(new Dictionary<Reindeer, ReindeerState>());

        foreach (var r in data.Reindeers)
        {
            state.States.Add(r, new ReindeerState(Type.None, 0, 0, 0));
        }

        for (var i = 0; i < iterations; i++)
        {
            state = data.Run(state);
        }

        return state.States.Max(x => x.Value.Distance);
    }

    private static DayData Parse(string[] lines)
    {
        var settings = new List<Reindeer>();

        foreach (var line in lines)
        {
            var m = LineRegex.Match(line);

            var name = m.Groups[1].Value;
            var speed = int.Parse(m.Groups[2].Value);
            var flyTime = int.Parse(m.Groups[3].Value);
            var restTime = int.Parse(m.Groups[4].Value);

            settings.Add(new (name, speed, flyTime, restTime));
        }

        return new DayData(settings);
    }

    private record Reindeer(string Name, int Speed, int FlyTime, int RestTime);

    private record State(Dictionary<Reindeer, ReindeerState> States) { }

    private record ReindeerState(Type Type, int Seconds, int Distance, int TotalSeconds);

    private enum Type
    {
        None,
        Fly,
        Rest
    }

    private record DayData(List<Reindeer> Reindeers)
    {
        public State Run(State state)
        {
            var newStates = new Dictionary<Reindeer, ReindeerState>();

            foreach (var (r, s) in state.States)
            {
                var isFly = s.Type != Type.Rest;

                if (isFly)
                {
                    if (s.Seconds >= r.FlyTime)
                    {
                        newStates.Add(r, s with
                        {
                            Type = Type.Rest,
                            Seconds = 1,
                            TotalSeconds = s.TotalSeconds + 1
                        });
                    }
                    else
                    {
                        newStates.Add(r, s with
                        {
                            Type = Type.Fly,
                            Seconds = s.Seconds + 1,
                            Distance = s.Distance + r.Speed,
                            TotalSeconds = s.TotalSeconds + 1
                        });
                    }
                }
                else
                {
                    if (s.Seconds >= r.RestTime)
                    {
                        newStates.Add(r, s with
                        {
                            Type = Type.Fly,
                            Seconds = 1,
                            Distance = s.Distance + r.Speed,
                            TotalSeconds = s.TotalSeconds + 1
                        });
                    }
                    else
                    {
                        newStates.Add(r, s with
                        {
                            Seconds = s.Seconds + 1,
                            TotalSeconds = s.TotalSeconds + 1
                        });
                    }
                }
            }

            return new State(newStates);
        }
    }
}
