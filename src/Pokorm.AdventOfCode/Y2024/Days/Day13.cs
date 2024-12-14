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

        var result = 0;

        foreach (var config in data.Configs)
        {
            result += Run(config);
        }

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        return result;
    }

    private int Run(Config config)
    {
        const int maxLength = 100;

        var coord = new Coord(0, 0);

        var minCost = 0;

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

        /*var subResult = new List<int>();

        subResult.AddRange(Run(0, coord, 0, config.A, config));

        subResult.AddRange(Run(0, coord, 0, config.B, config));

        return subResult;*/
    }

    private List<int> Run(int i, Coord coord, int tokens, Button btn, Config config)
    {
        if (i >= 100)
        {
            return [ ];
        }

        logger.LogDebug($"{i:00}: {btn.Name}");

        tokens += btn.TokenCost;
        coord = coord + btn.V;

        if (coord.X == config.PrizeCoord.X && coord.Y == config.PrizeCoord.Y)
        {
            return [ tokens ];
        }

        if (coord.X > config.PrizeCoord.X || coord.Y > config.PrizeCoord.Y)
        {
            return [ ];
        }

        var subResult = new List<int>();

        subResult.AddRange(Run(i + 1, coord, tokens, config.A, config));

        subResult.AddRange(Run(i + 1, coord, tokens, config.B, config));

        return subResult;
    }

    private static DayData Parse(string[] lines)
    {
        var configs = new List<Config>();

        foreach (var line in lines.Where(x => !string.IsNullOrWhiteSpace(x)).Chunk(3))
        {
            var a = ButtonARegex.Match(line[0]);
            var ax = int.Parse(a.Groups["x"].Value);
            var ay = int.Parse(a.Groups["y"].Value);

            var b = ButtonBRegex.Match(line[1]);
            var bx = int.Parse(b.Groups["x"].Value);
            var by = int.Parse(b.Groups["y"].Value);

            var p = PrizeRegex.Match(line[2]);
            var px = int.Parse(p.Groups["x"].Value);
            var py = int.Parse(p.Groups["y"].Value);

            configs.Add(new Config(new Button('A', new Vector(ax, ay), 3), new Button('B', new Vector(bx, by), 1), new Coord(px, py)));
        }

        return new DayData(configs);
    }

    private record Config(Button A, Button B, Coord PrizeCoord);

    private record Button(char Name, Vector V, int TokenCost);

    private record DayData(List<Config> Configs) { }
}
