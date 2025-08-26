namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/18
public class Day18(ILogger<Day18> logger)
{
    
    public long Solve(string[] lines, int steps)
    {
        var data = Parse(lines);

        for (var s = 0; s < steps; s++)
        {
            var d = data.Data.Select(x => x.ToList()).ToList();

            foreach (var (i, row) in d.Index())
            {
                foreach (var (j, l) in row.Index())
                {
                    var neib = new List<bool>();

                    //logger.LogInformation($"[{s}]: [{i}][{j}]");

                    for (var a = int.Max(0, i - 1); a <= int.Min(i + 1, data.Data.Count - 1); a++)
                    {
                        for (var b = int.Max(0, j - 1); b <= int.Min(j + 1, row.Count - 1); b++)
                        {
                            if (a == i && b == j)
                            {
                                continue;
                            }

                            //logger.LogInformation($" - [{a}][{b}] {data.Data[a][b]}");

                            neib.Add(d[a][b]);
                        }
                    }

                    var nextState = l ? neib.Count(x => x) is 2 or 3 : neib.Count(x => x) is 3;

                    //logger.LogInformation($" -> {nextState}");

                    data.Data[i][j] = nextState;
                }
            }
        }

        var result = data.Data.SelectMany(x => x).Count(x => x);

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
        List<List<bool>> data = [];

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            List<bool> lineData = [];

            foreach (var c in line)
            {
                lineData.Add(c == '#');
            }

            data.Add(lineData);
        }

        return new DayData(data);
    }

    private record DayData(List<List<bool>> Data) { }
}
