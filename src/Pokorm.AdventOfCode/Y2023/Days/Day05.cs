namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day05 : IDay
{
    private readonly IInputService inputService;

    public Day05(IInputService inputService) => this.inputService = inputService;

    public int SolveAsync()
    {
        var data = Parse();

        return (int) data.MapSeeds("seed", "location").Min();
    }

    public int SolveBonusAsync()
    {
        var data = Parse();

        return 0;
    }

    private Data Parse()
    {
        var lines = this.inputService.GetInputLines(2023, 5);

        var seeds = new List<long>();
        var maps = new List<Map>();
        string? from = null, to = null;
        var entries = new List<MapEntry>();

        foreach (var line in lines)
        {
            if (line.StartsWith("seeds"))
            {
                seeds = line.FullSplit(':')[1].FullSplit(' ').Select(long.Parse).ToList();

                continue;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                if (to is not null && from is not null)
                {
                    maps.Add(new Map(from, to, entries));
                    to = from = null;
                    entries = new List<MapEntry>();
                }
            }
            else
            {
                if (from is null && to is null)
                {
                    var fromTo = line.FullSplit(' ')[0].FullSplit('-');
                    from = fromTo[0];
                    to = fromTo[2];
                }
                else
                {
                    var ranges = line.FullSplit(' ').Select(long.Parse).ToArray();

                    entries.Add(new MapEntry(ranges[0], ranges[1], ranges[2]));
                }
            }
        }
        
        if (to is not null && from is not null)
        {
            maps.Add(new Map(from, to, entries));
        }

        return new Data(seeds, maps);
    }

    private record Data(List<long> Seeds, List<Map> Maps)
    {
        public IEnumerable<long> MapSeeds(string source, string dest)
        {
            foreach (var seed in this.Seeds)
            {
                var cursor = seed;

                foreach (var map in GetOrderedMaps(source, dest))
                {
                    cursor = map.MapSourceToDest(cursor);
                }

                yield return cursor;
            }
        }

        public IEnumerable<Map> GetOrderedMaps(string source, string dest)
        {
            return this.Maps.SkipWhile(x => x.From != source)
                       .TakeWhile(x => x.To != dest);
        }
    }

    private record Map(string From, string To, List<MapEntry> Entries)
    {
        public long MapSourceToDest(long input)
        {
            foreach (var entry in this.Entries)
            {
                if (entry.TryMap(input, out var entryResult))
                {
                    return entryResult;
                }
            }

            return input;
        }
    }

    private record MapEntry(long DestStart, long SourceStart, long RangeLength)
    {
        public bool TryMap(long input, out long result)
        {
            result = 0;

            var i = 0;
            for (long sourceCursor = this.SourceStart; sourceCursor < this.SourceStart + this.RangeLength; sourceCursor++)
            {
                if (input == sourceCursor)
                {
                    result = this.DestStart + i;

                    return true;
                }

                i++;
            }

            return false;
        }
    }
}
