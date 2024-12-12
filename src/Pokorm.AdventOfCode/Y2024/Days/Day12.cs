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

        var result = 0;

        return result;
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

        public long CalcRegionFencePrice(Region region) => CalcRegionArea(region) * CalcRegionPerimeter(region);

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

        public IEnumerable<Region> FindRegions()
        {
            var seen = new HashSet<Coord>();

            foreach (var (coord, c) in this.GardenPlots)
            {
                var coords = Recurse(coord, c);

                if (coords.Count == 0)
                {
                    continue;
                }

                yield return new (c, coords.ToHashSet());
            }

            List<Coord> Recurse(Coord coord, char c)
            {
                if (!seen.Add(coord))
                {
                    return [ ];
                }

                var result = new List<Coord>()
                {
                    coord
                };

                foreach (var siblingCoord in GetGardenPlotSiblings(coord, c))
                {
                    result.AddRange(Recurse(siblingCoord, c));
                }

                return result;
            }
        }

        public IEnumerable<Coord> GetGardenPlotSiblings(Coord coord, char c)
        {
            return coord.GetSiblings(dirs, this.Grid)
                        .Where(cr => this.GardenPlots[cr] == c)
                        .ToList();
        }
    }
}
