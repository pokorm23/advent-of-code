using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/13
public partial class Day13(ILogger<Day13> logger)
{
    [GeneratedRegex(@"Button A: X\+(?<x>\d+), Y\+(?<y>\d+)")]
    public static partial Regex ButtonARegex { get; }

    [GeneratedRegex(@"Button B: X\+(?<x>\d+), Y\+(?<y>\d+)")]
    public static partial Regex ButtonBRegex { get; }

    [GeneratedRegex(@"Prize: X=(?<x>\d+), Y=(?<y>\d+)")]
    public static partial Regex PrizeRegex { get; }

    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var result = 0L;

        foreach (var config in data.Configs)
        {
            result += Run(config);
        }

        return result;
    }

    public long SolvePartOneFast(string[] lines)
    {
        var data = Parse(lines);

        var result = 0L;

        var i = 0;
        
        foreach (var config in data.Configs)
        {
            using var _ = logger.BeginScope($"{i+1}/{data.Configs.Count}: ");
            
            logger.LogDebug($"Solving {config}");
            result += RunFast(config);
            i++;
        }

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines, true);

        var result = 0L;

        var i = 0;
        
        foreach (var config in data.Configs)
        {
            using var _ = logger.BeginScope($"{i+1}/{data.Configs.Count}: ");
            
            logger.LogDebug($"Solving {config}");
            result += RunFast(config);
            i++;
        }

        return result;
    }

    private long Run(Config config)
    {
        const long maxLength = 100;

        var coord = new Coord(0, 0);

        var minCost = 0L;

        for (var i = 0; i < maxLength; i++)
        {
            for (var j = 0; j < maxLength; j++)
            {
                var aPresses = i + 1;
                var bPresses = j + 1;

                var hitCoord = coord + config.A.V * aPresses + config.B.V * bPresses;

                if (hitCoord.X > config.PrizeCoord.X || hitCoord.Y > config.PrizeCoord.Y)
                {
                    break; // the j cycle
                }

                if (hitCoord.X != config.PrizeCoord.X || hitCoord.Y != config.PrizeCoord.Y)
                {
                    continue;
                }

                var cost = config.A.TokenCost * aPresses + config.B.TokenCost * bPresses;

                minCost = minCost == 0 ? cost : Math.Min(minCost, cost);
            }
        }

        return minCost;
    }

    private long RunFast(Config config)
    {
        long[] a = { config.A.V.X, config.A.V.Y };               
        long[] b = { config.B.V.X, config.B.V.Y };               
        long[] c = { config.PrizeCoord.X, config.PrizeCoord.Y }; 
        
        var coord = new Coord(0, 0);

        var minCost = 0L;
        for (long x = 1; x <= c[0] / a[0] + 1; x++)
        {
            for (long y = 1; y <= c[1] / b[1] + 1; y++)
            {
                if (x * a[0] + y * b[0] == c[0] && x * a[1] + y * b[1] == c[1])
                {
                    var aPresses = x;
                    var bPresses = y;

                    var hitCoord = coord + config.A.V * aPresses + config.B.V * bPresses;

                    if (hitCoord.X > config.PrizeCoord.X || hitCoord.Y > config.PrizeCoord.Y)
                    {
                        continue;
                    }

                    if (hitCoord.X != config.PrizeCoord.X || hitCoord.Y != config.PrizeCoord.Y)
                    {
                        continue;
                    }

                    var cost = config.A.TokenCost * aPresses + config.B.TokenCost * bPresses;

                    minCost = minCost == 0 ? cost : Math.Min(minCost, cost);
                }
            }
        }
        
        var aMax = Math.Max(config.PrizeCoord.X / config.A.V.X, config.PrizeCoord.Y / config.A.V.Y);


        /*for (var i = 0; i < aMax; i++)
        {
            var bMax = Math.Max((config.PrizeCoord.X - i * config.A.V.X) / config.B.V.X,
                (config.PrizeCoord.Y - i * config.A.V.Y) / config.B.V.Y);

            for (var j = 0; j < bMax; j++)
            {
                var aPresses = i + 1;
                var bPresses = j + 1;

                var hitCoord = coord + config.A.V * aPresses + config.B.V * bPresses;

                if (hitCoord.X > config.PrizeCoord.X || hitCoord.Y > config.PrizeCoord.Y)
                {
                    continue;
                }

                if (hitCoord.X != config.PrizeCoord.X || hitCoord.Y != config.PrizeCoord.Y)
                {
                    continue;
                }

                var cost = config.A.TokenCost * aPresses + config.B.TokenCost * bPresses;

                minCost = minCost == 0 ? cost : Math.Min(minCost, cost);
            }
        }*/

        return minCost;

        // X: a_x * a + b_x * b = p_x
        // Y: a_y * a + b_y * b = p_y

        // sol exists: gcd(a_x,b_x)|p_x && gcd(a_y,b_y)|p_y && 
        /*var coord = new Coord(0, 0);

        var minCost = 0L;

        for (var k = 1; k < Math.Max(aMax, bMax); k++)
        {
            var pairs = Enumerable.Range(0, k).SelectMany(x => Enumerable.Range(0, k).Select(y => (x, y)))
                                  .Where(t => t != (0, 0))
                                  .ToList();

            foreach (var (aPresses, bPresses) in pairs)
            {
                var hitCoord = coord + config.A.V * aPresses + config.B.V * bPresses;

                if (hitCoord.X > config.PrizeCoord.X || hitCoord.Y > config.PrizeCoord.Y)
                {
                    continue;
                }

                if (hitCoord.X != config.PrizeCoord.X || hitCoord.Y != config.PrizeCoord.Y)
                {
                    continue;
                }

                var cost = config.A.TokenCost * aPresses + config.B.TokenCost * bPresses;

                minCost = minCost == 0 ? cost : Math.Min(minCost, cost);
            }
        }

        return minCost;*/
    }

    private long Gcd(long a, long b)
    {
        if (a < b)
        {
            (a, b) = (b, a);
        }

        var e = Eea(a, b);

        return e.alpha * a + e.beta * b;
    }

    private (long r, long alpha, long beta, long q) Eea(long a, long b)
    {
        Debug.Assert(a >= b && b > 0);

        var t = new List<(long r, long alpha, long beta, long q)>();

        t.Add((a, 1, 0, -1));
        t.Add((b, 0, 1, a / b));

        var i = 2;
        var rk = 0L;

        while ((rk = t[i - 2].r - t[i - 1].q * t[i - 1].r) != 0)
        {
            var ak = t[i - 2].alpha - t[i - 1].q * t[i - 1].alpha;
            var bk = t[i - 2].beta - t[i - 1].q * t[i - 1].beta;
            var qk = t[i - 1].r / rk;

            t.Add((rk, ak, bk, qk));
            i++;
        }

        return t[^1];
    }

    private static DayData Parse(string[] lines, bool bonus = false)
    {
        var configs = new List<Config>();

        foreach (var line in lines.Where(x => !string.IsNullOrWhiteSpace(x)).Chunk(3))
        {
            var a = ButtonARegex.Match(line[0]);
            var ax = long.Parse(a.Groups["x"].Value);
            var ay = long.Parse(a.Groups["y"].Value);

            var b = ButtonBRegex.Match(line[1]);
            var bx = long.Parse(b.Groups["x"].Value);
            var by = long.Parse(b.Groups["y"].Value);

            var p = PrizeRegex.Match(line[2]);
            var px = long.Parse(p.Groups["x"].Value);
            var py = long.Parse(p.Groups["y"].Value);

            if (bonus)
            {
                px += 10000000000000L;
                py += 10000000000000L;
            }

            configs.Add(new Config(new Button('A', new Vector(ax, ay), 3), new Button('B', new Vector(bx, by), 1), new Coord(px, py)));
        }

        return new DayData(configs);
    }

    private record Config(Button A, Button B, Coord PrizeCoord);

    private record Button(char Name, Vector V, long TokenCost);

    private record DayData(List<Config> Configs) { }
}
