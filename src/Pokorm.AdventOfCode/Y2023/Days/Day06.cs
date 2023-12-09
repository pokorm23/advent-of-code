using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day06 : IDay
{
    private readonly IInputService inputService;

    public Day06(IInputService inputService) => this.inputService = inputService;

    public long Solve()
    {
        var data = Parse(false);

        return data.GetMarginOfError();
    }

    public long SolveBonus()
    {
        var data = Parse(true);

        return data.GetMarginOfError();
    }

    private Data Parse(bool ignoreKerning)
    {
        var lines = this.inputService.GetInputLines(GetType());

        var times = lines[0].FullSplit(':')[1].FullSplit(' ').Select(long.Parse).ToArray();
        var distances = lines[1].FullSplit(':')[1].FullSplit(' ').Select(long.Parse).ToArray();

        if (ignoreKerning)
        {
            times = [long.Parse(string.Join("", times.Select(x => x.ToString())))];
            distances = [long.Parse(string.Join("", distances.Select(x => x.ToString())))];
        }

        var races = times.Zip(distances).Select(x => new Race(x.First, x.Second)).ToList();

        var r = new Data(races);

        Trace.WriteLine($"Parsed: {r}");

        return r;
    }

    private record Data(List<Race> Races)
    {
        public long GetMarginOfError()
        {
            return this.Races.Select(x => x.GetHoldTimeToBeatRecord().Count())
                       .Aggregate(1L, (acc, x) => acc * x);
        }

        public override string ToString() => string.Join(Environment.NewLine, this.Races);
    }

    private record Race(long Time, long RecordDistance)
    {
        public IEnumerable<long> GetAllPossibleDistances()
        {
            for (var i = 0L; i < this.Time + 1; i++)
            {
                yield return (this.Time - i) * i;
            }
        }

        public IEnumerable<long> GetHoldTimeToBeatRecord()
        {
            foreach (var i in GetAllPossibleDistances())
            {
                if (i > this.RecordDistance)
                {
                    yield return i;
                }
            }
        }

        public override string ToString() => $"t={this.Time}ms / r={this.RecordDistance}mm";
    }
}
