namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/4
public class Day04
{
    public long Solve(string[] lines)
    {
        var width = 0;
        var height = lines.Length;
        var y = 0;
        var points = new Dictionary<Coord, char>();

        foreach (var line in lines)
        {
            var lineWidth = 0;

            foreach (var c in line)
            {
                var coord = new Coord(lineWidth, y);

                points.Add(coord, c);

                lineWidth++;
            }

            width = Math.Max(width, lineWidth);
            y++;
        }

        var board = new Board(width, height);

        var data = new BoardData(board, points);

        int[] possibleChangeInPos = [ -1, 0, 1 ];

        var vectors = possibleChangeInPos.SelectMany(x => possibleChangeInPos.Select(y => new Vector(x, y))).Where(x => x!= Vector.Zero).ToList();

        var result = 0;
        
        foreach (var (coord, value) in data.Data)
        {
            if (value != 'X')
            {
                continue;
            }

            foreach (var vector in vectors)
            {
                var potCoord = coord + (vector * 3);

                if (!data.Board.IsCoordValid(potCoord))
                {
                    continue;
                }

                var mChar = data.Board.TryGetCoordInDirection(coord, vector);

                if (mChar is null || data.GetPoint(mChar.Value).Value != 'M')
                {
                    continue;
                }
                
                var aChar = data.Board.TryGetCoordInDirection(coord, vector * 2);

                if (aChar is null || data.GetPoint(aChar.Value).Value != 'A')
                {
                    continue;
                }
                var sChar = data.Board.TryGetCoordInDirection(coord, vector * 3);

                if (sChar is null || data.GetPoint(sChar.Value).Value != 'S')
                {
                    continue;
                }

                result++;
            }
        }

        return result;
    }

    private record struct Vector(int X, int Y)
    {
        public static Vector Zero = new (0,0);
        public static Vector operator *(Vector c, int scale) => new Vector(c.X * scale, c.Y * scale);
    }

    private record struct Coord(int X, int Y)
    {
        public static Coord operator +(Coord c, Vector v) => new Coord(c.X + v.X, c.Y + v.Y);

    }

    private record BoardPoint(Coord Coord, char Value) { }

    private record Board(int Width, int Height)
    {
        public bool IsCoordValid(Coord source) => source.Y >= 0 && source.X >= 0 && source.Y < this.Height && source.X < this.Width;

        public Coord? TryGetCoordInDirection(Coord source, Vector vector)
        {
            if (!IsCoordValid(source))
            {
                throw new ArgumentOutOfRangeException(nameof(source), $"{source} range out in {this}");
            }

            var newCoord = new Coord(source.X + vector.X, source.Y + vector.Y);

            if (IsCoordValid(newCoord))
            {
                return newCoord;
            }

            return null;
        }

        public IEnumerable<Coord> GetSiblingCoords(Coord source)
        {
            if (!IsCoordValid(source))
            {
                throw new ArgumentOutOfRangeException(nameof(source), $"{source} range out in {this}");
            }

            int[] possibleChangeInPos = [ -1, 0, 1 ];

            var movements = possibleChangeInPos.SelectMany(x => possibleChangeInPos.Select(y => (x, y)));

            foreach (var (x, y) in movements)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                var newCoord = new Coord(source.X + x, source.Y + y);

                if (IsCoordValid(newCoord))
                {
                    yield return newCoord;
                }
            }
        }
    }

    private record BoardData(Board Board, Dictionary<Coord, char> Data)
    {
        public BoardPoint GetPoint(Coord source)
        {
            if (!this.Board.IsCoordValid(source))
            {
                throw new ArgumentOutOfRangeException(nameof(source), $"{source} range out in {this.Board}");
            }

            var point = this.Data.TryGetValue(source, out var v) ? v : (char) 0;

            return new BoardPoint(source, point);
        }
    }
}
