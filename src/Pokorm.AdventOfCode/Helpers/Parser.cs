namespace Pokorm.AdventOfCode.Helpers;

public static class Parser
{
    private static List<long> ParseNums(string input) => input.FullSplit(' ').Select(long.Parse).ToList();

    private static Grid ParseGrid(string[] lines, Action<Coord> onCoord)
    {
        var width = 0;
        var height = lines.Length;
        var y = 0;

        foreach (var line in lines)
        {
            var lineWidth = 0;

            foreach (var c in line)
            {
                var coord = new Coord(lineWidth, y);

                onCoord(coord);

                lineWidth++;
            }

            width = Math.Max(width, lineWidth);
            y++;
        }

        var board = new Grid(width, height);

        return board;
    }
}