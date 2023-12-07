using System.Security.Cryptography.X509Certificates;

var input = File.ReadAllText("Input.txt");
var total = 0;
var hands = new List<Hand>();

foreach (var line in input.Split("\r\n"))
{
    Console.WriteLine($"Line: {line}");

    var parts = line.Split(' ');

    hands.Add(new Hand(parts[0], int.Parse(parts[1])));

    Console.WriteLine();
}

hands.Sort(new HandComparer());

for (int i = 0; i < hands.Count; i++)
{
    total += hands[i].Bid * (hands.Count - i);
}

Console.WriteLine($"Total: {total}");


record Hand(string Cards, int Bid);

class HandComparer : Comparer<Hand>
{
    public override int Compare(Hand? x, Hand? y)
    {
        var xType = FindType(x.Cards);
        var yType = FindType(y.Cards);
        
        if (xType == yType)
        {
            return 0;
        }

        return xType - yType;
    }

    private int FindType(string cards)
    {
        var distinctCards = cards.ToArray().Distinct().ToArray();

        if (distinctCards.Length == 1)
        {
            // Five of a kind.
            return 0;
        }
        else if (distinctCards.Length == 2)
        {
            // Four of a kind or full house.
            var card1Count = cards.Count(c => c == distinctCards[0]);
            var card2Count = cards.Count(c => c == distinctCards[1]);

            if ((card1Count == 1) || (card2Count == 1))
            {
                // Four of a kind.
                return 1;
            }
            else
            {
                return 2;
            }
        }
        else if (distinctCards.Length == 3)
        {
            // 3 of a kind or 2 pair.
        }
        else if (distinctCards.Length == 4)
        {
            // A pair.
            return 5;
        }

        // All cards unique, high card.
        return 6;
    }
}