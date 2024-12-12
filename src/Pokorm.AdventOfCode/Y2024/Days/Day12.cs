namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/12
public class Day12
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var regions = data.FindRegions();

        return regions.Sum(x => data.CalcRegionFencePrice(x));
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var regions = data.FindRegions();

        return regions.Sum(x => data.CalcRegionFencePrice(x, true));
    }

    public static DayData Parse(string[] lines)
    {
        var gardenPlots = new Dictionary<Coord, char>();

        var grid = Parser.ParseGrid(lines, (c, coord) =>
        {
            gardenPlots.Add(coord, c);
        });

        return new DayData(grid, gardenPlots);
    }

    public record Region(char GardenPlot, HashSet<Coord> Coords);

    public record DayData(Grid Grid, Dictionary<Coord, char> GardenPlots)
    {
        private static readonly List<Vector> dirs = new[]
        {
            (0, 1),
            (1, 0),
            (-1, 0),
            (0, -1)
        }.Select(x => new Vector(x)).ToList();

        public long CalcRegionFencePrice(Region region, bool applyDiscount = false)
        {
            var size = applyDiscount ? CalcRegionSides(region) : CalcRegionPerimeter(region);

            return CalcRegionArea(region) * size;
        }

        public long CalcRegionArea(Region r) => r.Coords.Count;

        public long CalcRegionPerimeter(Region r)
        {
            var result = 0L;

            foreach (var c in r.Coords)
            {
                var siblingCount = GetGardenPlotSiblings(c, r.GardenPlot).Count();

                result += 4 - siblingCount;
            }

            return result;
        }

        public long CalcRegionSides(Region r)
        {
            var result = 0L;

            foreach (var d in dirs)
            {
                var edgeCoords = r.Coords.Where(x => this.Grid.TryGetCoordInDirection(x, d) is not { } s || this.GardenPlots[s] != r.GardenPlot)
                                  .ToHashSet();

                var seen = new HashSet<Coord>();

                var edgeSides = 0L;

                Vector[] otherDirs = [ d.ToRightRotated(), -d.ToRightRotated() ];

                foreach (var coord in edgeCoords)
                {
                    if (seen.Contains(coord))
                    {
                        continue;
                    }

                    _ = Recurse(coord, x => GetGardenPlotSiblings(x, r.GardenPlot, otherDirs).Where(y => edgeCoords.Contains(y)), seen);

                    edgeSides++;
                }

                result += edgeSides;
            }

            return result;
        }

        public IEnumerable<Region> FindRegions()
        {
            var seen = new HashSet<Coord>();

            foreach (var (coord, c) in this.GardenPlots)
            {
                var coords = Recurse(coord, x => GetGardenPlotSiblings(x, c), seen);

                if (coords.Count == 0)
                {
                    continue;
                }

                yield return new (c, coords.ToHashSet());
            }
        }

        private List<Coord> Recurse(Coord coord, Func<Coord, IEnumerable<Coord>> siblingFactory, HashSet<Coord> seen)
        {
            if (!seen.Add(coord))
            {
                return [ ];
            }

            var result = new List<Coord>()
            {
                coord
            };

            foreach (var siblingCoord in siblingFactory(coord))
            {
                result.AddRange(Recurse(siblingCoord, siblingFactory, seen));
            }

            return result;
        }

        public IEnumerable<Coord> GetGardenPlotSiblings(Coord coord, char c) => GetGardenPlotSiblings(coord, c, dirs);

        public IEnumerable<Coord> GetGardenPlotSiblings(Coord coord, char c, params IEnumerable<Vector> d)
        {
            return this.Grid.GetSiblings(coord, d)
                       .Where(cr => this.GardenPlots[cr] == c)
                       .ToList();
        }
    }
}
