namespace Pokorm.AdventOfCode.Helpers;

public static class Parser
{
    public static List<long> ParseNums(string input) => input.FullSplit(' ').Select(long.Parse).ToList();

    public static Grid ParseGrid(string[] lines, Action<char, Coord> onCoord)
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

                onCoord(c, coord);

                lineWidth++;
            }

            width = Math.Max(width, lineWidth);
            y++;
        }

        var board = new Grid(width, height);

        return board;
    }
}