var input = File.ReadAllText("Input.txt");
var lines = input.Split("\r\n");
var total = 0;
var numberOfCards = Enumerable.Repeat(1, lines.Length).ToArray();

for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
{
    var line = lines[lineIndex];
    
    Console.WriteLine($"Line: {line}");

    var parts = line.Split(':', '|');
    var winningNumbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var myNumbers = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var matchedNubers = winningNumbers.Intersect(myNumbers);

    Console.WriteLine($"Matched Numbers: {string.Join(' ', matchedNubers)}");

    if (matchedNubers.Count() == 1)
    {
        total += 1;
    }
    else
    {
        total += (int)Math.Pow(2, matchedNubers.Count() - 1);
    }

    // Increase the cards below this card 1 for each instance of this card we have.
    for (int i = 0; i < matchedNubers.Count(); i++)
    {
        numberOfCards[lineIndex + 1 + i] = numberOfCards[lineIndex + 1 + i] + numberOfCards[lineIndex];
    }
}

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Total cards: {numberOfCards.Sum(c => c)}");