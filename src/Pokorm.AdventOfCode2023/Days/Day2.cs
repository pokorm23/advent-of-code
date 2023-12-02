using System.Diagnostics;

namespace Pokorm.AdventOfCode2023;

public class Day2 : IDay
{
    private readonly IInputService inputService;

    public Day2(IInputService inputService) => this.inputService = inputService;

    public int Day => 2;

    public async Task<string> SolveAsync()
    {
        var lines = await this.inputService.GetInputLinesAsync(this.Day);

        var sum = 0;

        foreach (var line in lines)
        {
            sum += Solve(line, false);
        }

        return sum.ToString();
    }

    public async Task<string> SolveBonusAsync()
    {
        var lines = await this.inputService.GetInputLinesAsync(this.Day);

        var sum = 0;

        foreach (var line in lines)
        {
            sum += Solve(line, true);
        }

        return sum.ToString();
    }

    private int Solve(string line, bool bonus)
    {
        var game = Game.Parse(line);

        Debug.WriteLine($"Solving game: {game} ...");

        var criteria = new List<IGamePlayCriteria>()
        {
            new BagCapacityCriteria(12, 13, 14)
        };

        var possible = criteria.All(x => game.Plays.All(x.IsPossible));

        Debug.WriteLine($" - possible: {possible}");
        
        return possible ? game.Id : 0;
    }

    private record Game(int Id, List<GamePlay> Plays)
    {
        public override string ToString() => $"Game {Id}: {string.Join("; ", Plays)}";

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
                var throwDict = new Dictionary<CubeType, int>();

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

    private record GamePlay(Dictionary<CubeType, int> Cubes)
    {
        public override string ToString() => string.Join(", ", Cubes.Select(x => $"{x.Value} {x.Key.ToString().ToLower()}"));

        public int Count(CubeType type)
        {
            return this.Cubes.Where(x => x.Key == type).Sum(x => x.Value);
        }
    }

    private enum CubeType { Red, Green, Blue }

    private interface IGamePlayCriteria
    {
        bool IsPossible(GamePlay gamePlay);
    }

    // only 12 red cubes, 13 green cubes, and 14 blue cubes
    private class BagCapacityCriteria : IGamePlayCriteria
    {
        private readonly int redCount;
        private readonly int greenCount;
        private readonly int blueCount;

        public BagCapacityCriteria(int redCount, int greenCount, int blueCount)
        {
            this.redCount = redCount;
            this.greenCount = greenCount;
            this.blueCount = blueCount;
        }
        
        public bool IsPossible(GamePlay gamePlay) => gamePlay.Count(CubeType.Red) <= redCount && gamePlay.Count(CubeType.Green) <= greenCount && gamePlay.Count(CubeType.Blue) <= blueCount;
    }
}
