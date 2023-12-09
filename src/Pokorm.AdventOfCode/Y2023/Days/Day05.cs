using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day05 : IDay
{
    private readonly IInputService inputService;

    public Day05(IInputService inputService) => this.inputService = inputService;

    public long Solve()
    {
        var data = Parse();

        return (int) data.MapSeeds("seed", "location").Min();
    }

    public long SolveBonus()
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
        public long End => this.Start + Math.Max(0, this.Length - 1);

        public static Range FromStartEnd(long start, long end)
        {
            if (end < start)
            {
                throw new ArgumentException("end must be greater than start");
            }

            return new Range(start, end - start + 1);
        }

        public override string ToString() => $"{this.Start}..{this.End}({this.Length})";
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

            var maps = GetOrderedMaps(source, dest).ToList();

            Trace.WriteLine($"Maps: {string.Join(", ", maps)}");
            Trace.WriteLine(null);

            foreach (var map in maps)
            {
                Trace.WriteLine($"Mapping {map}, input = {runRanges.Count}");

                var iterResult = map.MapSourceToDest(runRanges).ToList();

                runRanges = iterResult;

                Trace.WriteLine($"--- Mapped {map}, output = {runRanges.Count}");

                Trace.WriteLine(null);
            }

            return runRanges;
        }

        public long MapSeed(long seed, string source, string dest)
        {
            return GetOrderedMaps(source, dest).Aggregate(seed, (current, map) => map.MapSourceToDest(current));
        }

        public IEnumerable<Map> GetOrderedMaps(string source, string dest)
        {
            var inLoop = false;
            
            foreach (var map in this.Maps)
            {
                if (map.From == source)
                {
                    yield return map;
                    inLoop = true;
                }

                if (map.To == dest)
                {
                    yield return map;
                    yield break;
                }

                if (inLoop)
                {
                    yield return map;
                }
            }
        }
    }

    private record Map(string From, string To, List<MapEntry> Entries)
    {
        public IEnumerable<Range> MapSourceToDest(IEnumerable<Range> srcRanges)
        {
            foreach (var src in srcRanges)
            {
                Trace.WriteLine($"-> input = {src}");

                var sortedEntries = this.Entries.OrderBy(x => x.Source.Start).ToList();

                var mappedRanges = new List<(Range Input, Range Output)>();

                foreach (var entry in sortedEntries)
                {
                    Trace.Write($"  - trying entry = {entry}");

                    var (input, transformedRange) = entry.MapRange(src);

                    if (transformedRange is not null && input is not null)
                    {
                        mappedRanges.Add((input, transformedRange));
                        Trace.WriteLine($" -> {input}");
                    }
                    else
                    {
                        Trace.WriteLine(" -> no match");
                    }
                }

                if (mappedRanges.Count == 0)
                {
                    Trace.WriteLine($" - direct = {src}");

                    yield return src;

                    continue;
                }

                // iterate gaps between input matches
                for (var i = 0; i < mappedRanges.Count; i++)
                {
                    // try gap before first
                    if (i == 0 && mappedRanges[i].Input.Start > src.Start)
                    {
                        var preGapStart = src.Start;
                        var preGapEnd = mappedRanges[i].Input.Start - 1;

                        var preGap = Range.FromStartEnd(preGapStart, preGapEnd);

                        Trace.WriteLine($" - direct (pre-gap) = {preGap}");

                        yield return preGap;
                    }

                    var (input, output) = mappedRanges[i];

                    Trace.WriteLine($" - transform = {input} -> {output}");

                    yield return output; // yield transformed

                    var startOfNext = i != mappedRanges.Count - 1
                                          ? mappedRanges[i + 1].Input.Start
                                          : src.End + 1;

                    if (startOfNext > src.End)
                    {
                        continue;
                    }

                    var gapStart = input.End + 1;
                    var gapEnd = startOfNext - 1;

                    if (gapStart >= gapEnd)
                    {
                        continue;
                    }

                    var gap = Range.FromStartEnd(gapStart, gapEnd);

                    Trace.WriteLine($" - direct (gap) = {gap}");

                    yield return gap;
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

        public override string ToString() => $"{this.From} -> {this.To} ({this.Entries.Count})";
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

            var inputRange = Range.FromStartEnd(start, end);
            var transformedRange = Range.FromStartEnd(shiftedStart, shiftedEnd);

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

        public override string ToString() => $"{this.Source}{(this.Diff >= 0 ? "+" : "")}{this.Diff}";
    }
}
