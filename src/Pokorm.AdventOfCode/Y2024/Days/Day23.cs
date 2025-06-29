namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/23
public class Day23(ILogger<Day23> logger)
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var threeLoops = new HashSet<ThreePair>();

        var vertices = data.Connections.SelectMany(x => new List<Key>()
        {
            x.A,
            x.B
        }).Distinct();

        foreach (var vertex in vertices)
        {
            var n = data.Connections.Where(x => x.A == vertex || x.B == vertex)
                        .Select(x => x.A == vertex ? x.B : x.A)
                        .Distinct()
                        .ToList();

            for (var i = 0; i < n.Count; i++)
            {
                for (var j = i + 1; j < n.Count; j++)
                {
                    var a = n[i];
                    var b = n[j];

                    var d = new Pair(a, b);

                    if (data.Connections.Contains(d))
                    {
                        threeLoops.Add(new (vertex, a, b));
                    }
                }
            }
        }

        var possibles = threeLoops.Where(x => x.A.Value.StartsWith('t')
                                              || x.B.Value.StartsWith('t')
                                              || x.C.Value.StartsWith('t'))
                                  .ToHashSet();

        var result = possibles.Count;

        return result;
    }

    public string SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var vertices = data.Connections.SelectMany(x => new List<Key>()
        {
            x.A,
            x.B
        }).Distinct().ToList();

        var r = FindMaxFullGraph_Iterative(new Vertices(vertices.ToHashSet()), data.Connections);

        return string.Join(",", r.V.Select(x => x.Value).Order());
    }

    private Vertices FindMaxFullGraph_Iterative(Vertices v, HashSet<Pair> edges)
    {
        var q = new Stack<Vertices>();
        var cache = new HashSet<string>();
        var maxFull = default(Vertices);

        foreach (var key in v.V)
        {
            q.Push(new Vertices([ key ]));
        }

        while (q.Count > 0)
        {
            var vs = q.Pop();
            var d = vs.V.Count;

            foreach (var nv in v.V)
            {
                // already in vertices
                if (vs.V.Contains(nv))
                {
                    continue;
                }

                var newVs = new Vertices([ ..vs.V, nv ]);

                // already processed
                if (cache.Contains(newVs.HashKey))
                {
                    continue;
                }

                var isFull = true;

                // vs is full graph, verify if adding nv also is full graph
                foreach (var k in vs.V)
                {
                    if (edges.Contains(new Pair(nv, k)))
                    {
                        continue;
                    }

                    isFull = false;

                    break;
                }

                if (isFull)
                {
                    q.Push(newVs);

                    if (maxFull is null || newVs.V.Count > maxFull.V.Count)
                    {
                        maxFull = newVs;

                        // assumption
                        if (maxFull.V.Count == 13)
                        {
                            q.Clear();
                            break;
                        }
                    }
                }

                cache.Add(newVs.HashKey);
            }
        }

        return maxFull ?? throw new Exception("max not found");
    }

    private record Vertices
    {
        public virtual bool Equals(Vertices? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.HashKey == this.HashKey;
        }

        public override int GetHashCode() => this.HashKey.GetHashCode();

        public HashSet<Key> V { get; }

        public string HashKey { get; }

        public Vertices(HashSet<Key> V)
        {
            this.V = V;
            this.HashKey = string.Join("", V.Select(x => x.Value).Order());
        }
    }

    private static DayData Parse(string[] lines)
    {
        var c = new HashSet<Pair>();

        foreach (var line in lines)
        {
            var split = line.FullSplit('-');

            var keys = split.Select(x => new Key(x)).ToList();

            c.Add(new (keys[0], keys[1]));
        }

        return new DayData(c);
    }

    private record DayData(HashSet<Pair> Connections) { }

    private record Pair
    {
        public Key A { get; }

        public Key B { get; }

        public int Num { get; }

        public Pair(Key A, Key B)
        {
            this.A = A >= B ? A : B;
            this.B = A >= B ? B : A;
            this.Num = 1000 * this.A.Num + this.B.Num;
        }

        public virtual bool Equals(Pair? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Num == other.Num;
        }

        public override int GetHashCode() => this.Num.GetHashCode();
    }

    private record struct Key : IComparable<Key>, IComparable
    {
        public int CompareTo(Key other) => this.Num.CompareTo(other.Num);

        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            return obj is Key other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Key)}");
        }

        public static bool operator <(Key left, Key right) => left.CompareTo(right) < 0;

        public static bool operator >(Key left, Key right) => left.CompareTo(right) > 0;

        public static bool operator <=(Key left, Key right) => left.CompareTo(right) <= 0;

        public static bool operator >=(Key left, Key right) => left.CompareTo(right) >= 0;

        public readonly bool Equals(Key other) => this.Num == other.Num;

        public readonly override int GetHashCode() => this.Num.GetHashCode();

        public string Value { get; }

        public int Num { get; }

        public Key(string value)
        {
            this.Value = value;
            var b = 26;
            var (c1, c2) = value.Length == 1 ? (value[0], 'z') : (value[0], value[1]);
            this.Num = (int) (c2 - 'a') + b * (c1 - 'a');
        }
    }

    private record ThreePair(Key A, Key B, Key C)
    {
        public virtual bool Equals(ThreePair? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return (this.A, this.B, this.C) == (other.A, other.B, other.C)
                   || (this.A, this.C, this.B) == (other.A, other.B, other.C)
                   || (this.B, this.A, this.C) == (other.A, other.B, other.C)
                   || (this.B, this.C, this.A) == (other.A, other.B, other.C)
                   || (this.C, this.B, this.A) == (other.A, other.B, other.C)
                   || (this.C, this.A, this.B) == (other.A, other.B, other.C);
        }

        public override int GetHashCode() => 0;
    }
}
