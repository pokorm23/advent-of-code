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

        return (int) data.MapSeedsAsRanges("seed", "location").Min();
    }

    private Data Parse()
    {
        var lines = this.inputService.GetInputLines(2023, 5);

        var seeds = new List<Seed>();
        var maps = new List<Map>();
        string? from = null, to = null;
        var entries = new List<MapEntry>();

        foreach (var line in lines)
        {
            if (line.StartsWith("seeds"))
            {
                var seedsNums = line.FullSplit(':')[1].FullSplit(' ').Select(long.Parse).ToList();

                foreach (var longs in seedsNums.Chunk(2))
                {
                    seeds.Add(new Seed(longs[0], longs[1]));
                }
                
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

    record Seed(long Start, long Lenght);

    private record Data(List<Seed> Seeds, List<Map> Maps)
    {
        public IEnumerable<long> MapSeeds(string source, string dest)
        {
            return this.Seeds.SelectMany(x => new [] {x.Start, x.Lenght}).Select(seed => MapSeed(seed, source, dest));
        }

        public IEnumerable<long> MapSeedsAsRanges(string source, string dest)
        {
            foreach (var (start, lenght) in this.Seeds)
            {
                for (var i = start; i < start + lenght; i++)
                {
                    yield return MapSeed(i, source, dest);
                }
            }
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
        public long MapSourceToDest(long input)
        {
            foreach (var entry in this.Entries)
            {
                if (entry.TryMap(input, out var result))
                {
                    return result;
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

            if (input < this.SourceStart || input >= this.SourceStart + this.RangeLength)
            {
                return false;
            }

            result = this.DestStart - this.SourceStart + input;

            return true;
        }
    }
}
