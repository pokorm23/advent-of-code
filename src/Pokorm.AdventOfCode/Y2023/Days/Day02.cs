using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day02 : IDay
{
    private readonly IInputService inputService;

    public Day02(IInputService inputService) => this.inputService = inputService;

    public int SolveAsync()
    {
        var lines = this.inputService.GetInputLines(2023, 2);

        var sum = 0;

        foreach (var line in lines)
        {
            sum += Solve(line, false);
        }

        return sum;
    }

    public int SolveBonusAsync()
    {
        var lines = this.inputService.GetInputLines(2023, 2);

        var sum = 0;

        foreach (var line in lines)
        {
            sum += Solve(line, true);
        }

        return sum;
    }

    private int Solve(string line, bool bonus)
    {
        var game = Game.Parse(line);

        Debug.WriteLine($"Solving game: {game} ...");

        var criteria = new BagCapacityCriteria(12, 13, 14);

        var possible = game.Plays.All(criteria.IsPossible);

        Debug.WriteLine($" - possible: {possible}");

        if (bonus)
        {
            var min = criteria.MinToBePossible(game);

            return min.Power();
        }

        return possible ? game.Id : 0;
    }

    private record Game(int Id, List<GamePlay> Plays)
    {
        public override string ToString() => $"Game {this.Id}: {string.Join("; ", this.Plays)}";

        public static Game Parse(string input)
        {
            var keyValue = input.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (keyValue.Length != 2)
            {
                throw new Exception();
            }

            var gameId = int.Parse(keyValue[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);

            var l = new List<GamePlay>();

            var plays = keyValue[1].Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            foreach (var play in plays)
            {
                var throwDict = new CubeSet();

                var throws = play.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                foreach (var t in throws)
                {
                    var stat = t.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                    var type = Enum.Parse<CubeType>(stat[1], true);

                    var count = int.Parse(stat[0]);

                    throwDict.TryAdd(type, 0);

                    throwDict[type] += count;
                }

                l.Add(new GamePlay(throwDict));
            }

            return new Game(gameId, l);
        }
    }

    private record GamePlay(CubeSet Cubes)
    {
        public override string ToString() => string.Join(", ", this.Cubes.Select(x => $"{x.Value} {x.Key.ToString().ToLower()}"));

        public int Count(CubeType type)
        {
            return this.Cubes.Where(x => x.Key == type).Sum(x => x.Value);
        }
    }

    private enum CubeType { Red, Green, Blue }

    private interface IGamePlayCriteria
    {
        bool IsPossible(GamePlay gamePlay);

        CubeSet MinToBePossible(Game game);
    }

    private class CubeSet : Dictionary<CubeType, int>
    {
        public CubeSet() { }

        public CubeSet(Dictionary<CubeType, int> dict) : base(dict) { }

        public int Power()
        {
            return this.Aggregate(1, (acc, x) => acc * x.Value);
        }
    }

    private class BagCapacityCriteria : IGamePlayCriteria
    {
        private readonly int blueCount;
        private readonly int greenCount;
        private readonly int redCount;

        public BagCapacityCriteria(int redCount, int greenCount, int blueCount)
        {
            this.redCount = redCount;
            this.greenCount = greenCount;
            this.blueCount = blueCount;
        }

        public bool IsPossible(GamePlay gamePlay) => gamePlay.Count(CubeType.Red) <= this.redCount && gamePlay.Count(CubeType.Green) <= this.greenCount && gamePlay.Count(CubeType.Blue) <= this.blueCount;

        public CubeSet MinToBePossible(Game game)
        {
            var maxes = game.Plays.SelectMany(x => x.Cubes)
                            .GroupBy(x => x.Key)
                            .Select(x => (x.Key, x.Select(y => y.Value).Max()));

            return new CubeSet(maxes.ToDictionary(x => x.Key, x => x.Item2));
        }
    }
}
