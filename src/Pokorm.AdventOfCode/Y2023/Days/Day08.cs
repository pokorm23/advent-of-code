using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day08 : IDay
{
    private readonly IInputService inputService;

    public Day08(IInputService inputService) => this.inputService = inputService;

    public long Solve()
    {
        var data = Parse();

        var steps = 0;
        var currentPosition = new Position("AAA");

        while (true)
        {
            foreach (var d in data.Directions)
            {
                steps++;
                
                currentPosition = data.GetNextPosition(currentPosition, d);

                if (currentPosition.IsFinish)
                {
                    return steps;
                }
            }
        }
    }

    public long SolveBonus()
    {
        var data = Parse();

        return 0;
    }

    private Data Parse()
    {
        var lines = this.inputService.GetInputLines(GetType());

        bool isReadingDirections = true;
        List<Direction> dirs = [];
        Dictionary<Position, (Position Left, Position Right)> map = new ();
        
        foreach (var line in lines)
        {
            if (isReadingDirections && string.IsNullOrWhiteSpace(line))
            {
                isReadingDirections = false;
                continue;
            }

            if (isReadingDirections)
            {
                dirs.AddRange(line.Where(x => x is 'R' or 'r' or 'L' or 'l').Select(x => x switch
                {
                    'R' or 'r' => Direction.Right,
                    'L' or 'l' => Direction.Left,
                    _ => throw new UnreachableException()
                }));
            }
            else
            {
                // TJS = (LFP, HKT)
                var eqSplit = line.FullSplit('=');

                var key = eqSplit[0].Trim().ToUpper();
                var pair = eqSplit[1].Trim().TrimStart('(').TrimEnd(')').FullSplit(',');
                var left = pair[0].Trim().ToUpper();
                var right = pair[1].Trim().ToUpper();
                
                map.Add(new (key), (new (left), new (right)));
            }
        }

        var result = new Data(dirs, map);

        Console.WriteLine($"Parsed: {result}");

        return result;
    }

    private record Data(
        List<Direction> Directions,
        Dictionary<Position, (Position Left, Position Right)> NavigationMap)
    {
        public Position GetNextPosition(Position current, Direction direction)
        {
            return direction switch
            {
                Direction.Left  => NavigationMap[current].Left,
                Direction.Right => NavigationMap[current].Right,
                _               => throw new UnreachableException()
            };
        }
    }
    
    enum Direction { Left, Right }

    record Position(string Value)
    {
        public bool IsStart => Value is "AAA";
        public bool IsFinish => Value is "ZZZ";
    }
}
