using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day06 : IDay
{
    private readonly IInputService inputService;

    public Day06(IInputService inputService) => this.inputService = inputService;

    public int Solve()
    {
        var data = Parse(false);

        return data.GetMarginOfError();
    }

    public int SolveBonus()
    {
        var data = Parse(true);

        return data.GetMarginOfError();
    }

    private Data Parse(bool ignoreKerning)
    {
        var lines = this.inputService.GetInputLines(GetType());

        var times = lines[0].FullSplit(':')[1].FullSplit(' ').Select(int.Parse).ToArray();
        var distances = lines[1].FullSplit(':')[1].FullSplit(' ').Select(int.Parse).ToArray();

        if (ignoreKerning)
        {
            times = [int.Parse(string.Join("", times.Select(x => x.ToString())))];
            distances = [int.Parse(string.Join("", distances.Select(x => x.ToString())))];
        }
        
        var races = times.Zip(distances).Select(x => new Race(x.First, x.Second)).ToList();

        var r = new Data(races);
        
        Trace.WriteLine($"Parsed: {r}");

        return r;
    }

    private record Data(List<Race> Races)
    {
        public int GetMarginOfError()
        {
            return this.Races.Select(x => x.GetHoldTimeToBeatRecord().Count())
                       .Aggregate(1, (acc, x) => acc * x);
        }

        public override string ToString() => string.Join(Environment.NewLine, Races);
    }

    private record Race(int Time, int RecordDistance)
    {
        public IEnumerable<int> GetAllPossibleDistances()
        {
            foreach (var i in Enumerable.Range(0, this.Time + 1))
            {
                yield return (this.Time - i) * i;
            }
        }

        public IEnumerable<int> GetHoldTimeToBeatRecord()
        {
            foreach (var i in GetAllPossibleDistances())
            {
                if (i > this.RecordDistance)
                {
                    yield return i;
                }
            }
        }

        public override string ToString() => $"t={Time}ms / r={RecordDistance}mm";
    }
}
