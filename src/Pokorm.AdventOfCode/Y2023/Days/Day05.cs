using System.Collections.Frozen;

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
            return this.Seeds.Select(seed => MapSeed(seed, source, dest));
        }

        public long MapSeed(long seed, string source, string dest)
        {
            return GetOrderedMaps(source, dest).Aggregate(seed, (current, map) => map.MapSourceToDest(current));
        }

        public IEnumerable<Map> GetOrderedMaps(string source, string dest)
        {
            return this.Maps.SkipWhile(x => x.From != source)
                       .TakeWhile(x => x.To != dest);
        }
    }

    private record Map(string From, string To, List<MapEntry> Entries)
    {
        private IDictionary<long, long>? lookup;

        public long MapSourceToDest(long input)
        {
            this.lookup ??= CreateLookup();

            return this.lookup.TryGetValue(input, out var entryResult) ? entryResult : input;
        }

        public IDictionary<long, long> CreateLookup()
        {
            var dict = new Dictionary<long, long>();

            foreach (var entry in this.Entries.Select(x => x.CreateLookup()))
            {
                foreach (var (key, value) in entry)
                {
                    dict.Add(key, value);
                }
            }

            return dict.ToFrozenDictionary();
        }
    }

    private record MapEntry(long DestStart, long SourceStart, long RangeLength)
    {
        private IDictionary<long, long>? lookup;

        public IDictionary<long, long> CreateLookup()
        {
            var dict = new Dictionary<long, long>();

            var i = 0;

            for (var sourceCursor = this.SourceStart; sourceCursor < this.SourceStart + this.RangeLength; sourceCursor++)
            {
                dict.Add(sourceCursor, this.DestStart + i);
                i++;
            }

            return dict.ToFrozenDictionary();
        }

        public bool TryMap(long input, out long result)
        {
            this.lookup ??= CreateLookup();

            return this.lookup.TryGetValue(input, out result);
        }
    }
}
