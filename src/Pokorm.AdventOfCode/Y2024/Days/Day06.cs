namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/6
public class Day06
{
    private static BoardData Parse(string[] lines)
    {
        var width = 0;
        var height = lines.Length;
        var y = 0;
        var points = new Dictionary<Coord, BoardPointType>();
        BoardGuardPoint? boardGuardPoint = null;

        foreach (var line in lines)
        {
            var lineWidth = 0;

            foreach (var c in line)
            {
                var coord = new Coord(lineWidth, y);

                (BoardPointType, BoardGuardPoint?) point = c switch
                {
                    '.'   => (BoardPointType.Free, null),
                    '#'   => (BoardPointType.Obstacle, null),
                    '^'   => (BoardPointType.Free, new BoardGuardPoint(coord, BoardGuardDirection.Top)),
                    '>'   => (BoardPointType.Free, new BoardGuardPoint(coord, BoardGuardDirection.Right)),
                    '<'   => (BoardPointType.Free, new BoardGuardPoint(coord, BoardGuardDirection.Left)),
                    'v'   => (BoardPointType.Free, new BoardGuardPoint(coord, BoardGuardDirection.Bottom)),
                    var _ => throw new Exception()
                };

                points.Add(coord, point.Item1);

                if (boardGuardPoint is not null && point.Item2 is not null)
                {
                    throw new Exception();
                }

                if (point.Item2 is not null)
                {
                    boardGuardPoint = point.Item2;
                }

                lineWidth++;
            }

            width = Math.Max(width, lineWidth);
            y++;
        }

        var board = new Board(width, height);

        return new BoardData(board, points, boardGuardPoint!);
    }

    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var guard = data.Guard;

        var seenCoords = new HashSet<Coord>();

        do
        {
            seenCoords.Add(guard.Position);
            guard = data.MoveGuard(guard);
        } while (guard is not null);

        return seenCoords.Count;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var result = 0;

        var guard = data.Guard;

        var putObstacles = new HashSet<Coord>();

        do
        {
            var nextCoord = data.MoveGuard(guard)?.Position;

            if (nextCoord is not null && nextCoord != guard.Position && putObstacles.Add(nextCoord.Value))
            {
                var newData = data.Data.ToDictionary();

                newData[nextCoord.Value] = BoardPointType.Obstacle;

                var dataWithObstacle = data with
                {
                    Guard = guard,
                    Data = newData
                };

                if (dataWithObstacle.IsGuardLooped(guard))
                {
                    result++;
                }
            }

            guard = data.MoveGuard(guard);
        } while (guard is not null);

        return result;
    }

    private record struct Vector(int X, int Y)
    {
        public static Vector Zero = new Vector(0, 0);

        public static Vector operator *(Vector c, int scale) => new Vector(c.X * scale, c.Y * scale);
    }

    private record struct Coord(int X, int Y)
    {
        public static Coord operator +(Coord c, Vector v) => new Coord(c.X + v.X, c.Y + v.Y);
    }

    private enum BoardGuardDirection
    {
        Top,
        Left,
        Right,
        Bottom
    }

    private enum BoardPointType
    {
        Free,
        Obstacle
    }

    private record BoardGuardPoint(Coord Position, BoardGuardDirection Direction)
    {
        public Vector GetVector()
        {
            return this.Direction switch
            {
                BoardGuardDirection.Bottom => new (0, 1),
                BoardGuardDirection.Top    => new (0, -1),
                BoardGuardDirection.Right  => new (1, 0),
                BoardGuardDirection.Left   => new (-1, 0),
                var _                      => throw new Exception()
            };
        }

        public BoardGuardDirection GetRotatedDirection()
        {
            return this.Direction switch
            {
                BoardGuardDirection.Bottom => BoardGuardDirection.Left,
                BoardGuardDirection.Top    => BoardGuardDirection.Right,
                BoardGuardDirection.Right  => BoardGuardDirection.Bottom,
                BoardGuardDirection.Left   => BoardGuardDirection.Top,
                var _                      => throw new Exception()
            };
        }
    }

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
    }

    private record BoardData(Board Board, Dictionary<Coord, BoardPointType> Data, BoardGuardPoint Guard)
    {
        public BoardGuardPoint? MoveGuard(BoardGuardPoint guard)
        {
            var nextCoord = this.Board.TryGetCoordInDirection(guard.Position, guard.GetVector());

            if (nextCoord is null)
            {
                return null;
            }

            var nextPointType = this.Data[nextCoord.Value];

            if (nextPointType is BoardPointType.Free)
            {
                return guard with
                {
                    Position = nextCoord.Value
                };
            }

            if (nextPointType is BoardPointType.Obstacle)
            {
                return guard with
                {
                    Direction = guard.GetRotatedDirection()
                };
            }

            throw new Exception();
        }

        public void Run(BoardGuardPoint initial, Func<BoardGuardPoint, bool> action)
        {
            var guard = initial;

            do
            {
                var shouldContinue = action(guard);

                if (!shouldContinue)
                {
                    return;
                }

                guard = MoveGuard(guard);
            } while (guard is not null);
        }

        public bool IsGuardLooped(BoardGuardPoint initial)
        {
            var loop = false;
            var guardPositions = new HashSet<BoardGuardPoint>();

            Run(initial, currentGuard =>
            {
                // loop detected
                if (!guardPositions.Add(currentGuard))
                {
                    loop = true;
                    return false;
                }

                return true;
            });

            return loop;
        }
    }
}
