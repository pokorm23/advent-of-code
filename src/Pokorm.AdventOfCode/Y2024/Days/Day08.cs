namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/8
public class Day08
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var antiNodes = data.GetAllAntiNodes(false);

        return antiNodes.Count;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var antiNodes = data.GetAllAntiNodes(true);

        return antiNodes.Count;
    }

    private static DayData Parse(string[] lines)
    {
        var width = 0;
        var height = lines.Length;
        var y = 0;
        var points = new Dictionary<Coord, MapPosition>();

        foreach (var line in lines)
        {
            var lineWidth = 0;

            foreach (var c in line)
            {
                var coord = new Coord(lineWidth, y);

                points.Add(coord, c switch
                {
                    '.'   => new FreeMapPosition(coord),
                    var _ => new AntennaMapPosition(coord, c)
                });

                lineWidth++;
            }

            width = Math.Max(width, lineWidth);
            y++;
        }

        var board = new Board(width, height);

        return new DayData(board, points);
    }

    private record struct Vector(int X, int Y)
    {
        public static Vector Zero = new Vector(0, 0);

        public static Vector operator *(Vector c, int scale) => new Vector(c.X * scale, c.Y * scale);

        public static Vector operator -(Vector c) => c * -1;
    }

    private record struct Coord(int X, int Y)
    {
        public static Coord operator +(Coord c, Vector v) => new Coord(c.X + v.X, c.Y + v.Y);

        public static Coord operator -(Coord c, Vector v) => new Coord(c.X - v.X, c.Y - v.Y);

        public static Vector operator -(Coord c, Coord v) => new Vector(c.X - v.X, c.Y - v.Y);
    }

    private record Board(int Width, int Height)
    {
        public bool IsCoordValid(Coord source) => source.Y >= 0 && source.X >= 0 && source.Y < this.Height && source.X < this.Width;

        public Coord? TryGetCoordInDirection(Coord source, Vector vector)
        {
            if (!IsCoordValid(source))
            {
                throw new ArgumentOutOfRangeException(nameof(source), $"{source} range out in {this}");
            }

            var newCoord = new Coord(source.X + vector.X, source.Y + vector.Y);

            if (IsCoordValid(newCoord))
            {
                return newCoord;
            }

            return null;
        }
    }

    private abstract record MapPosition(Coord Coord);

    private record FreeMapPosition(Coord Coord) : MapPosition(Coord);

    private record AntennaMapPosition(Coord Coord, char Id) : MapPosition(Coord);

    private record AntennaPair(char Id, Coord A, Coord B)
    {
        public Vector Vector => this.B - this.A;

        public IEnumerable<Coord> GetFirstAntiNodes(Board bounds)
        {
            var d = this.Vector;

            return ((Coord[]) [ this.A - d, this.B + d ]).Where(bounds.IsCoordValid);
        }

        public IEnumerable<Coord> GetAntiNodes(Board bounds)
        {
            var d = this.Vector;
            var c = this.A;

            yield return this.A;

            while (true)
            {
                c += d;

                if (!bounds.IsCoordValid(c))
                {
                    break;
                }

                yield return c;
            }

            c = this.A;

            while (true)
            {
                c -= d;

                if (!bounds.IsCoordValid(c))
                {
                    break;
                }

                yield return c;
            }
        }
    }

    private record DayData(Board Board, Dictionary<Coord, MapPosition> Points)
    {
        public IEnumerable<AntennaPair> PairAntennas()
        {
            foreach (var p in this.Points.Values
                                  .OfType<AntennaMapPosition>()
                                  .GroupBy(x => x.Id))
            {
                var seenCombinations = new HashSet<HashSet<Coord>>(EqualityComparer<HashSet<Coord>>.Create((x, y) => x!.SetEquals(y!), x => 0));

                foreach (var pos in p)
                {
                    foreach (var otherPos in p.Where(x => x != pos))
                    {
                        if (!seenCombinations.Add([ pos.Coord, otherPos.Coord ]))
                        {
                            continue;
                        }

                        yield return new (p.Key, pos.Coord, otherPos.Coord);
                    }
                }
            }
        }

        public ISet<Coord> GetAllAntiNodes(bool includeHigherHarmonics)
        {
            var allPairs = PairAntennas().ToList();

            var antiNodes = new HashSet<Coord>();

            foreach (var pair in allPairs)
            {
                var an = !includeHigherHarmonics ? pair.GetFirstAntiNodes(this.Board)
                             : pair.GetAntiNodes(this.Board);

                foreach (var anc in an)
                {
                    antiNodes.Add(anc);
                }
            }

            return antiNodes;
        }
    }
}
