var input = File.ReadAllText("Input.txt");
var total = 0;

foreach (var line in input.Split("\r\n"))
{
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
}

Console.WriteLine($"Total: {total}");