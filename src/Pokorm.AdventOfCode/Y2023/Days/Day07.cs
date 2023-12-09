using System.Diagnostics;

namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day07 : IDay
{
    private readonly IInputService inputService;

    public Day07(IInputService inputService) => this.inputService = inputService;

    public long Solve()
    {
        var data = Parse();

        return data.GetTotalWinnings();
    }

    public long SolveBonus()
    {
        var data = Parse();

        return data.GetTotalWinnings();
    }

    private Game Parse()
    {
        var lines = this.inputService.GetInputLines(GetType());

        var plays = new List<PlayedHand>();

        foreach (var line in lines)
        {
            var playSplit = line.FullSplit(' ');
            var cards = playSplit[0].ToArray();
            var bid = int.Parse(playSplit[1]);

            var hand = cards.Select(x => new Card(x, x switch
            {
                'A' => 12,
                'K' => 11,
                'Q' => 10,
                'J' => 9,
                'T' => 8,
                '9' => 7,
                '8' => 6,
                '7' => 5,
                '6' => 4,
                '5' => 3,
                '4' => 2,
                '3' => 1,
                '2' => 0,
                _   => throw new Exception("Unknown card")
            })).ToArray();

            if (hand.Length != 5)
            {
                throw new Exception("Invalid hand");
            }

            plays.Add(new PlayedHand(new Hand(hand[0], hand[1], hand[2], hand[3], hand[4]), bid));
        }

        var r = new Game(plays);

        Trace.WriteLine($"Parsed: {r}");

        return r;
    }

    private record Game(List<PlayedHand> Plays)
    {
        public IEnumerable<PlayedHand> GetOrdered()
        {
            return this.Plays.OrderBy(x => x.Hand.GetHandType())
                       .ThenBy(x => x.Hand.GetStrength());
        }

        public long GetTotalWinnings()
        {
            var result = 0L;
            var rankCursor = 1;

            foreach (var play in GetOrdered())
            {
                Trace.WriteLine($"Rank {rankCursor}: {play}");

                result += rankCursor * play.Bid;

                rankCursor++;
            }

            return result;
        }

        public override string ToString() => $"{string.Join(Environment.NewLine, this.Plays)}";
    }

    private record PlayedHand(Hand Hand, int Bid)
    {
        public override string ToString() => $"{this.Hand} {this.Bid}";
    }

    private record Hand(Card Card1, Card Card2, Card Card3, Card Card4, Card Card5)
    {
        public override string ToString() => $"{this.Card1}{this.Card2}{this.Card3}{this.Card4}{this.Card5}";

        // O(1)
        public HandType GetHandType()
        {
            var cards = new[]
            {
                this.Card1,
                this.Card2,
                this.Card3,
                this.Card4,
                this.Card5
            };

            var grouped = cards.GroupBy(x => x.Label).Select(x => x.ToArray()).ToArray();

            if (grouped.Any(x => x.Length == 5))
            {
                return HandType.FiveOfAKind;
            }

            if (grouped.Any(x => x.Length == 4))
            {
                return HandType.FourOfAKind;
            }

            if (grouped.Any(x => x.Length == 3))
            {
                return grouped.Any(x => x.Length == 2) ? HandType.FullHouse : HandType.ThreeOfAKind;
            }

            if (grouped.Count(x => x.Length == 2) == 2)
            {
                return HandType.TwoPair;
            }

            if (grouped.Any(x => x.Length == 2))
            {
                return HandType.OnePair;
            }

            return HandType.HighCard;
        }

        // O(1)
        public int GetStrength()
        {
            var result = 0;
            const int multiplier = 16;

            result += this.Card5.Strength;
            result += this.Card4.Strength * multiplier;
            result += this.Card3.Strength * multiplier * multiplier;
            result += this.Card2.Strength * multiplier * multiplier * multiplier;
            result += this.Card1.Strength * multiplier * multiplier * multiplier * multiplier;

            return result;
        }
    }

    private enum HandType
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    private record Card(char Label, int Strength)
    {
        public override string ToString() => $"{this.Label}";
    }
}
