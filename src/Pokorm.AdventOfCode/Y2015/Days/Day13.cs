using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/13
public partial class Day13
{
    private readonly ILogger<Day13> logger;

    [GeneratedRegex(@"(?<name1>\w+) would (?<type>\w+) (?<amount>\d+) happiness units by sitting next to (?<name2>\w+)\.")]
    public static partial Regex LineRegex { get; }

    public Day13(ILogger<Day13> logger) => this.logger = logger;

    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        foreach (var s in data.GetAllSetting())
        {
            var total = s.Sum(x => x.AppliedSettings.Sum(y => y.Amount * (y.IsGain ? 1 : -1)));

            this.logger.LogDebug($"{total}: {string.Join(", ", s.Select(x => x.Name))}");

            result = Math.Max(result, total);
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
        var settings = new List<Setting>();

        foreach (var line in lines)
        {
            var m = LineRegex.Match(line);

            var name1 = m.Groups[1].Value;
            var type = m.Groups[2].Value;
            var amount = int.Parse(m.Groups[3].Value);
            var name2 = m.Groups[4].Value;

            settings.Add(new (name1, type == "gain", amount, name2));
        }

        return new DayData(settings);
    }

    private record Setting(string TargetName, bool IsGain, int Amount, string SourceName);

    private record Seat(string Name, List<Setting> AppliedSettings);

    private record DayData(List<Setting> Settings)
    {
        public IEnumerable<List<Seat>> GetAllSetting()
        {
            var allNames = this.Settings.Select(s => s.SourceName).Concat(this.Settings.Select(s => s.TargetName))
                               .Distinct()
                               .ToList();

            var possibles = Get(allNames);

            foreach (var possible in possibles)
            {
                var result = new List<Seat>();

                foreach (var (i, n) in possible.Index())
                {
                    var left = i == 0 ? possible[^1] : possible[i - 1];
                    var right = i == possible.Count - 1 ? possible[0] : possible[i + 1];

                    var settings = new List<Setting>();

                    settings.AddRange(this.Settings.Where(x => x.TargetName == n && x.SourceName == left));
                    settings.AddRange(this.Settings.Where(x => x.TargetName == n && x.SourceName == right));

                    var seat = new Seat(n, settings);

                    result.Add(seat);
                }

                yield return result;
            }
        }

        private IEnumerable<List<string>> Get(List<string> s)
        {
            if (s.Count <= 1)
            {
                yield return s;

                yield break;
            }

            for (var i = 0; i < s.Count; i++)
            {
                var butFirst = s.Select((x, i) => (i, x)).Where(x => x.i != i).Select(x => x.x).ToList();

                var subPos = Get(butFirst).ToList();

                foreach (var p in subPos)
                {
                    yield return [ s[i], ..p ];
                }
            }
        }
    }
}
