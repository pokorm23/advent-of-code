namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day05 : IDay
{
    private readonly IInputService inputService;

    public Day05(IInputService inputService) => this.inputService = inputService;

    public int Solve()
    {
        var data = Parse();

        return (int) data.MapSeeds("seed", "location").Min();
    }

    public int SolveBonus()
    {
        var data = Parse();

        return (int) data.MapSeedsAsRanges("seed", "location").Select(x => x.Start).Min();
    }

    private Data Parse()
    {
        var lines = this.inputService.GetInputLines(2023, 5);

        var seeds = new List<Range>();
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
                    seeds.Add(new Range(longs[0], longs[1]));
                }

                continue;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                if (to is not null && from is not null)
                {
                    maps.Add(new Map(from, to, entries));
                    to = from = null;
                    entries = [];
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

                    var src = new Range(ranges[1], ranges[2]);
                    var diff = ranges[0] - src.Start;

                    entries.Add(new MapEntry(src, diff));
                }
            }
        }

        if (to is not null && from is not null)
        {
            maps.Add(new Map(from, to, entries));
        }

        return new Data(seeds, maps);
    }

    private record Range(long Start, long Length)
    {
        public long End => this.Start + this.Length - 1;
    }

    private record Data(List<Range> Seeds, List<Map> Maps)
    {
        public IEnumerable<long> MapSeeds(string source, string dest)
        {
            return this.Seeds.SelectMany(x => new[]
            {
                x.Start,
                x.Length
            }).Select(seed => MapSeed(seed, source, dest));
        }

        public IEnumerable<Range> MapSeedsAsRanges(string source, string dest)
        {
            var runRanges = this.Seeds.ToList();

            foreach (var map in GetOrderedMaps(source, dest))
            {
                var iterResult = map.MapSourceToDest(runRanges).ToList();

                runRanges = iterResult;
            }

            return runRanges;
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
        public IEnumerable<Range> MapSourceToDest(IEnumerable<Range> srcRanges)
        {
            foreach (var src in srcRanges)
            {
                var sortedEntries = this.Entries.OrderBy(x => x.Source.Start).ToList();

                var mappedRanges = new List<(Range Input, Range Output)>();

                foreach (var entry in sortedEntries)
                {
                    var (input, transformedRange) = entry.MapRange(src);

                    if (transformedRange is not null && input is not null)
                    {
                        mappedRanges.Add((input, transformedRange));
                    }
                }

                if (mappedRanges.Count == 0)
                {
                    yield return src;
                    continue;
                }
                
                // iterate gaps betwwen input matches
                for (var i = 0; i < mappedRanges.Count; i++)
                {
                    var (input, output) = mappedRanges[i];
                    // i = 0; 17-17
                    // i = 1; 20-20
                    // i = 2; 27-31
                    // i = 3; 48-50

                    yield return output; // yield transformed

                    var startOfNext = i != mappedRanges.Count - 1
                                          ? mappedRanges[i + 1].Input.Start
                                          : src.End + 1;

                    if (startOfNext > src.End)
                    {
                        continue;
                    }

                    var gapStart = input.End + 1; // 18
                    var gapEnd = startOfNext - 1; // 19

                    // i = 0; (20 - 1) - (17 + 1) + 1 = 2
                    var gapLength = gapEnd - gapStart + 1;
                    
                    // yield gap
                    yield return new Range(input.End + 1, gapLength);
                }
            }
        }

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

    private record MapEntry(Range Source, long Diff)
    {
        public (Range? Input, Range? Output) MapRange(Range src)
        {
            var dest = this.Source;

            if (dest.Start <= src.Start && dest.Start + dest.Length < src.Start)
            {
                return default; // outofbounds
            }

            if (dest.Start > src.Start + src.Length)
            {
                return default; // outofbounds
            }

            var start = Math.Max(dest.Start, src.Start);
            var end = Math.Min(dest.End, src.End);

            var shiftedStart = this.Diff + start;
            var shiftedEnd = this.Diff + end;
            var length = shiftedEnd - shiftedStart + 1;

            var inputRange = new Range(start, length);
            var transformedRange = new Range(shiftedStart, length);

            return (inputRange, transformedRange);
        }

        public bool TryMap(long input, out long result)
        {
            result = 0;

            if (input < this.Source.Start || input >= this.Source.Start + this.Source.Length)
            {
                return false;
            }

            result = input + this.Diff;

            return true;
        }
    }
}
