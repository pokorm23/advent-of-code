namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/23
public class Day23(ILogger<Day23> logger)
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var threeLoops = new HashSet<ThreePair>();

        var vertices = data.Connections.SelectMany(x => new List<string>()
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

        var possibles = threeLoops.Where(x => x.A.StartsWith('t')
                                              || x.B.StartsWith('t')
                                              || x.C.StartsWith('t'))
                                  .ToHashSet();

        var result = possibles.Count;

        return result;
    }

    public string SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var vertices = data.Connections.SelectMany(x => new List<string>()
        {
            x.A,
            x.B
        }).Distinct().ToList();

        var allEdges = GetAllEdges(vertices).ToHashSet();

        var complEdges = allEdges.ToHashSet();

        complEdges.ExceptWith(data.Connections);

        var edgeStack = new Stack<Pair>(complEdges);

        var ctx = new SearchContext();

        FindMaxFullGraph(vertices.ToHashSet(), edgeStack, ctx);

        return string.Join(",", ctx.CurrentMaxFullGraph.Order());
    }

    private void FindMaxFullGraph(HashSet<string> vertices, Stack<Pair> complEdges, SearchContext ctx)
    {
        // trivial case
        if (vertices.Count <= 3)
        {
            return;
        }

        // ex. bigger full graph already
        if (vertices.Count <= ctx.CurrentMaxFullGraph.Count)
        {
            return;
        }

        // found bigger
        if (complEdges.Count == 0)
        {
            ctx.TryToSet(vertices);

            return;
        }

        // division by removing compl edge and then
        // recursively run on each sub-graph without one of the vertices from the removed edge
        var edge = complEdges.Pop();

        if (IsFullGraph(vertices, complEdges.ToHashSet()))
        {
            ctx.TryToSet(vertices);

            return;
        }

        FindMaxFullGraph(vertices.Where(x => x != edge.A).ToHashSet(), new Stack<Pair>(complEdges), ctx);
        FindMaxFullGraph(vertices.Where(x => x != edge.B).ToHashSet(), new Stack<Pair>(complEdges), ctx);
    }

    private static bool IsFullGraph(HashSet<string> vertices, HashSet<Pair> edges)
    {
        var veEd = edges.SelectMany(x => new List<string>()
                        {
                            x.A,
                            x.B
                        })
                        .ToHashSet();

        return veEd.SequenceEqual(vertices);
    }

    private record SearchContext()
    {
        public HashSet<string> CurrentMaxFullGraph { get; set; } = [ ];

        public void TryToSet(HashSet<string> vertices)
        {
            if (vertices.Count > this.CurrentMaxFullGraph.Count)
            {
                this.CurrentMaxFullGraph = vertices.ToHashSet();
            }
        }
    }

    private static IEnumerable<Pair> GetAllEdges(IReadOnlyCollection<string> vertices)
    {
        var n = vertices.ToList();

        for (var i = 0; i < n.Count; i++)
        {
            for (var j = i + 1; j < n.Count; j++)
            {
                var a = n[i];
                var b = n[j];

                var d = new Pair(a, b);

                yield return d;
            }
        }
    }

    private static DayData Parse(string[] lines)
    {
        var c = new HashSet<Pair>();

        foreach (var line in lines)
        {
            var split = line.FullSplit('-');
            c.Add(new (split[0], split[1]));
        }

        return new DayData(c);
    }

    private record DayData(HashSet<Pair> Connections) { }

    private record Pair(string A, string B)
    {
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

            return (this.A, this.B) == (other.A, other.B)
                   || (this.B, this.A) == (other.A, other.B);
        }

        public override int GetHashCode() => 0;
    }

    private record ThreePair(string A, string B, string C)
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
