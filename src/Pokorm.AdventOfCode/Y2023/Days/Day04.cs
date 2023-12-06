using System.Text;

namespace Pokorm.AdventOfCode.Y2023.Days;

public class Day04 : IDay
{
    private readonly IInputService inputService;

    public Day04(IInputService inputService) => this.inputService = inputService;

    public int SolveAsync()
    {
        var set = Parse();

        return set.Cards.Sum(x => x.GetPoints());
    }

    public int SolveBonusAsync()
    {
        var set = Parse();

        var allCards = set.GetCardsWithCopies().ToList();
        
        return allCards.Count;
    }

    private CardSet Parse()
    {
        var lines = this.inputService.GetInputLines(2023, 4);

        return new CardSet(lines.Select(Parser.ParseLine).ToList());
    }

    record CardSet(IReadOnlyCollection<Card> Cards)
    {
        public IEnumerable<Card> GetCardsWithCopies()
        {
            var dict = new int[Cards.Count];

            var i = 0;
            
            foreach (var card in this.Cards)
            {
                var newCards = 1;
                
                yield return card;

                var currentCopies = dict[i];

                for (var j = 0; j < currentCopies; j++)
                {
                    newCards++;
                    yield return card;
                }

                var matches = card.GetNumberOfMatches();

                for (var j = 0; j < matches; j++)
                {
                    dict[i + j + 1] += newCards;
                }

                i++;
            }
        }
    }

    record Card(int Number, IReadOnlyCollection<int> PlayNumbers, IReadOnlyCollection<int> WinNumbers)
    {
        public int GetNumberOfMatches()
        {
            var dict = new Dictionary<int, int>();

            var matches = 0;
            
            foreach (var playNumber in this.PlayNumbers)
            {
                foreach (var winNumber in this.WinNumbers)
                {
                    if (playNumber == winNumber)
                    {
                        dict.TryAdd(playNumber, 0);

                        dict[playNumber]++;

                        matches++;
                    }
                }
            }

            return matches;
        }
        
        public int GetPoints()
        {
            return (int) Math.Pow(2, GetNumberOfMatches() - 1);
        }
    }

    private static class Parser
    {
        // Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
        public static Card ParseLine(string line)
        {
            var headBodySplit = line.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var id = int.Parse(headBodySplit[0]
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1]);
            
            var deckSplit = headBodySplit[1].Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var play = deckSplit[0];
            var win = deckSplit[1];
            
            return new Card(id,
                play.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList(),
                win.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());
        }
    }
}
