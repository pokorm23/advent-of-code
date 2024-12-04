namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/4
public class Day04
{
    public long Solve(string[] lines)
    {
        var width = 0;
        var height = lines.Length;
        var y = 0;
        var points = new List<BoardPoint>();

        foreach (var line in lines)
        {
            var lineWidth = 0;

            foreach (var c in line)
            {
                var coord = new Coord(lineWidth, y);

                points.Add(new (coord, c));

                lineWidth++;
            }

            width = Math.Max(width, lineWidth);
            y++;
        }

        var board = new Board(width, height);

        var data = new BoardData(board, points);

        return FindString("XMAS", data).Count();
    }

    private IEnumerable<Result> FindString(string text, BoardData data)
    {
        foreach (var boardPoint in data.Data)
        {
            foreach (var result in FindString(text, boardPoint, data))
            {
                Console.WriteLine($"Found on {boardPoint}: {string.Join(",", result.Points.Select(x => $"({x.Coord.X},{x.Coord.Y})"))}");
                yield return result;
            }
        }
    }

    private IEnumerable<Result> FindString(string text, BoardPoint boardPoint, BoardData data)
    {
        int[] possibleChangeInPos = [ -1, 0, 1 ];

        var vectors = possibleChangeInPos.SelectMany(x => possibleChangeInPos.Select(y => (x, y))).Where(x => x!= (0,0));

        foreach (var v in vectors)
        {
            var seq = text.AsSpan();
            var stack = new Stack<BoardPoint>();
            stack.Push(boardPoint);

            foreach (var result in FindString(seq, stack, v, data))
            {
                yield return result;
            }
        }
    }

    private IEnumerable<Result> FindString(ReadOnlySpan<char> seqToFind, Stack<BoardPoint> stack, (int X, int Y) vector, BoardData data)
    {
        if (seqToFind.Length == 0)
        {
            return [ new Result([ ..stack.Reverse() ]) ];
        }

        var matched = seqToFind[0] == stack.Peek().Value;

        if (!matched)
        {
            return [ ];
        }

        if (seqToFind.Length == 1)
        {
            return [ new Result([ ..stack.Reverse() ]) ];
        }

        var sourceCoord = stack.Peek();

        var potentialCoord = data.Board.TryGetCoordInDirection(sourceCoord.Coord, vector);

        if (potentialCoord is null)
        {
            return [ ];
        }

        var result = new List<Result>();

        var subSeq = seqToFind[1..];
        stack.Push(data.GetPoint(potentialCoord.Value));

        result.AddRange(FindString(subSeq, stack, vector, data));

        stack.Pop();

        return result;
    }

    private record struct Coord(int X, int Y);

    private record struct Result(List<BoardPoint> Points);

    private record BoardPoint(Coord Coord, char Value) { }

    private record Board(int Width, int Height)
    {
        public bool IsCoordValid(Coord source) => source.Y >= 0 && source.X >= 0 && source.Y < this.Height && source.X < this.Width;

        public Coord? TryGetCoordInDirection(Coord source, (int X, int Y) vector)
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

    private record BoardData(Board Board, List<BoardPoint> Data)
    {
        public BoardPoint GetPoint(Coord source)
        {
            if (!this.Board.IsCoordValid(source))
            {
                throw new ArgumentOutOfRangeException(nameof(source), $"{source} range out in {this.Board}");
            }

            var point = this.Data.FirstOrDefault(x => x.Coord == source);

            return point ?? new BoardPoint(source, (char) 0);
        }
    }
}
