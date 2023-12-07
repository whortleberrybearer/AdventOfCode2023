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

for (var i = 0; i < hands.Count; i++)
{
    total += hands[i].Bid * (i + 1);
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
            for (var i = 0; i < x.Cards.Length; i++)
            {
                var xValue = FindCardValue(x.Cards[i]);
                var yValue = FindCardValue(y.Cards[i]);

                if (xValue != yValue)
                {
                    return xValue - yValue;
                }
            }
        }

        return yType - xType;
    }
    
    private int FindType(string cards)
    {
        var distinctCards = cards.Replace("J", string.Empty).ToArray().Distinct().ToArray();

        if (distinctCards.Length == 0)
        {
            // All cards must be 'J'.
            return FindHandValue(cards, new char[] { 'J' });
        }
        
        if (cards.Any(c => c == 'J'))
        {
            var bestHandValue = 10;
            
            foreach (var distinctCard in distinctCards)
            {
                var jokerSwapedCards = cards.Replace('J', distinctCard);
                var handValue = FindHandValue(jokerSwapedCards, distinctCards);

                if (handValue < bestHandValue)
                {
                    bestHandValue = handValue;
                }
            }

            return bestHandValue;
        }
        
        return FindHandValue(cards, distinctCards);
    }

    private static int FindHandValue(string cards, char[] distinctCards)
    {
        if (distinctCards.Length == 1)
        {
            // Five of a kind.
            return 0;
        }
        else if (distinctCards.Length == 2)
        {
            // If 4 of 1 card, four of a kind otherwise full house.
            return distinctCards.Any(dc => cards.Count(c => dc == c) == 4) ? 1 : 2;
        }
        else if (distinctCards.Length == 3)
        {
            // If 3 of 1 card, 3 a kind otherwise 2 pair.
            return distinctCards.Any(dc => cards.Count(c => dc == c) == 3) ? 3 : 4;
        }
        else if (distinctCards.Length == 4)
        {
            // A pair.
            return 5;
        }

        // All cards unique, high card.
        return 6;
    }

    private int FindCardValue(char card)
    {
        return card switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 1,
            'T' => 10,
            _ => int.Parse(card.ToString())
        };
    }
}