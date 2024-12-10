using System.Collections.Concurrent;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/10
public class Day10
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var heads = data.GetTrailheads();

        var result = 0;

        foreach (var headPos in heads)
        {
            result += data.GetTrailCountFromPosition(headPos);
        }

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var heads = data.GetTrailheads();

        var result = 0;

        foreach (var headPos in heads)
        {
            result += data.GetPositionRating(headPos);
        }

        return result;
    }

    private static BoardData Parse(string[] lines)
    {
        var width = 0;
        var height = lines.Length;
        var y = 0;
        var points = new Dictionary<Coord, Position>();

        foreach (var line in lines)
        {
            var lineWidth = 0;

            foreach (var c in line)
            {
                var coord = new Coord(lineWidth, y);

                var num = c == '.' ? -1 : int.Parse(c.ToString());

                points.Add(coord, new Position(coord, num));

                lineWidth++;
            }

            width = Math.Max(width, lineWidth);
            y++;
        }

        var board = new Size(width, height);

        return new BoardData(board, points);
    }

    private record struct Vector(int X, int Y)
    {
        public Vector((int, int) tuple) : this(tuple.Item1, tuple.Item2) { }

        public static Vector Zero = new Vector(0, 0);

        public static Vector operator *(Vector c, int scale) => new Vector(c.X * scale, c.Y * scale);
    }

    private record struct Coord(int X, int Y)
    {
        public static Coord operator +(Coord c, Vector v) => new Coord(c.X + v.X, c.Y + v.Y);
    }

    private record Size(int Width, int Height)
    {
        public bool IsIn(Coord source) => source.Y >= 0 && source.X >= 0 && source.Y < this.Height && source.X < this.Width;
    }

    private record Position(Coord Coord, int Height)
    {
        public virtual bool Equals(Position? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Coord.Equals(other.Coord);
        }

        public override int GetHashCode() => this.Coord.GetHashCode();
    }

    private record Trail(List<Position> Positions)
    {
        public virtual bool Equals(Trail? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Positions.SequenceEqual(other.Positions);
        }

        public override int GetHashCode() => 0;
    }

    private record BoardData(Size Size, Dictionary<Coord, Position> Points)
    {
        public IEnumerable<Position> GetTrailheads()
        {
            return this.Points.Where(x => x.Value.Height == 0).Select(x => x.Value);
        }

        private (List<Position> Ends, ConcurrentDictionary<Position, List<Position>> LookbackMap) GetTrailFromPosition(Position position) // BFS
        {
            var q = new Queue<Position>();

            q.Enqueue(position);

            // pos -> isOpen
            var status = new ConcurrentDictionary<Position, bool>();

            status.TryAdd(position, true);

            var d = new ConcurrentDictionary<Position, int>();

            var p = new ConcurrentDictionary<Position, List<Position>>();

            var t = new List<HashSet<Position>>();

            while (q.Count > 0)
            {
                var v = q.Dequeue();

                var next = GetNextPositions(v).ToHashSet();

                t.Add(next);

                foreach (var w in next)
                {
                    if (!status.TryGetValue(w, out var _))
                    {
                        status.TryAdd(w, true);

                        var vd = d.TryGetValue(v, out var x) ? x : 0;

                        d.AddOrUpdate(w, vd + 1, (_, c) => vd + 1);

                        q.Enqueue(w);
                    }

                    p.AddOrUpdate(w, [ v ], (_, c) =>
                    {
                        c.Add(v);

                        return c;
                    });
                }

                status.AddOrUpdate(v, false, (_, _) => false);
            }

            var full = d.Where(x => x.Value == 9)
                        .Select(x => x.Key)
                        .ToList();

            return (full, p);
        }

        public int GetTrailCountFromPosition(Position position) => GetTrailFromPosition(position).Ends.Count;

        public int GetPositionRating(Position position)
        {
            var (ends, lookback) = GetTrailFromPosition(position);

            var trails = new List<Trail>();

            foreach (var end in ends)
            {
                var pos = end;

                var subTrails = GetTrails(pos, lookback);

                foreach (var subTrail in subTrails)
                {
                    trails.Add(subTrail);
                }
            }

            static IEnumerable<Trail> GetTrails(Position pos, ConcurrentDictionary<Position, List<Position>> lookback)
            {
                var back = lookback.TryGetValue(pos, out var s) ? s : [ ];

                if (back.Count == 0)
                {
                    return [ new Trail([ pos ]) ];
                }

                var trails = new List<Trail>();

                foreach (var end in back)
                {
                    var subTrails = GetTrails(end, lookback);

                    var newOnes = new List<Trail>();

                    foreach (var subTrail in subTrails)
                    {
                        newOnes.Add(new Trail([ ..subTrail.Positions, end ]));
                    }

                    trails.AddRange(newOnes);
                }

                return trails;
            }

            return trails.Count;
        }

        private IEnumerable<Position> GetNextPositions(Position position)
        {
            var dirs = new[]
            {
                (0, 1),
                (1, 0),
                (0, -1),
                (-1, 0)
            }.Select(x => new Vector(x));

            foreach (var vector in dirs)
            {
                var newCoord = position.Coord + vector;

                if (!this.Size.IsIn(newCoord))
                {
                    continue;
                }

                var pos = this.Points[newCoord];

                if (pos.Height == position.Height + 1)
                {
                    yield return pos;
                }
            }
        }
    }
}
