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

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
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
