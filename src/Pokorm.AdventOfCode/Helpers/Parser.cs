namespace Pokorm.AdventOfCode.Helpers;

public static class Parser
{
    public static List<long> ParseNums(string input) => input.FullSplit(' ').Select(long.Parse).ToList();

    public static Grid ParseGrid(string[] lines, Action<char, Coord> onCoord)
    {
        return ParseValuedGrid(lines, (c, coord) =>
        {
            onCoord(c, coord);

            return c;
        });
    }

    public static Grid<T> ParseValuedGrid<T>(string[] lines, Func<char, T> valueFactory) => ParseValuedGrid(lines, (c, _) => valueFactory(c));

    public static Grid<T> ParseValuedGrid<T>(string[] lines, Func<char, Coord, T> valueFactory)
    {
        var width = 0;
        var height = lines.Length;
        var y = 0;
        var values = new Dictionary<Coord, T>();

        foreach (var line in lines)
        {
            var lineWidth = 0;

            foreach (var c in line)
            {
                var coord = new Coord(lineWidth, y);

                var value = valueFactory(c, coord);

                values.Add(coord, value);

                lineWidth++;
            }

            width = Math.Max(width, lineWidth);
            y++;
        }

        var board = new Grid<T>(values, width, height);

        return board;
    }
}
