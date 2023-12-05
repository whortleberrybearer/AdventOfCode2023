var input = File.ReadAllText("Input.txt");
var total = 0;
var numbers = new Dictionary<string, string>()
{
    { "one", "1" },
    { "two", "2" },
    { "three", "3" },
    { "four", "4" },
    { "five", "5" },
    { "six", "6" },
    { "seven", "7" },
    { "eight", "8" },
    { "nine", "9" },
};

foreach (var line in input.Split("\r\n"))
{
    Console.WriteLine($"Line: {line}");

    // Replace any of the string numbers in the string with the digit.  Need to ensure string like "eightwo" are 82.  This was not clear from the
    // instructions (thanks internet).
    string newLine = line.ToLower();

    for (int i = 0; i < newLine.Length; i++)
    {
        foreach (var number in numbers)
        {
            if (string.Concat(newLine.Skip(i)).StartsWith(number.Key))
            {
                newLine = string.Concat(newLine.Take(i)) + number.Value + newLine.Substring(i + 1);
                break;
            }
        }
    }


    var first = string.Empty;
    var last = string.Empty;

    Console.WriteLine($"New line: {newLine}");

    for (int i = 0; i < newLine.Length; i++)
    {
        if (char.IsNumber(newLine[i]))
        {
            last = newLine[i].ToString();

            if (first == string.Empty)
            {
                first = last;
            }            
        }
    }

    Console.WriteLine($"First: {first}");
    Console.WriteLine($"Last: {last}");

    var value = int.Parse($"{first}{last}");

    Console.WriteLine($"Value: {value}");

    total += value;

    Console.WriteLine();
}

Console.WriteLine($"Total: {total}");