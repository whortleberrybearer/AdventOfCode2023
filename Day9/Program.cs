var input = File.ReadAllText("Input.txt");
var total = 0;

foreach (var line in input.Split("\r\n"))
{
    Console.WriteLine($"Line: {line}");

    var parts = line.Split(' ').Select(c => int.Parse(c)).ToList();
    var differences = parts.ToList();
    var lastNumbers = new List<int>();

    do
    {
        lastNumbers.Insert(0, differences.Last());

        var numbers = differences;
        differences = new List<int>();

        for (var i = 0; i < numbers.Count - 1; i++)
        {
            differences.Add(numbers[i + 1] - numbers[i]);
        }

        Console.WriteLine($"Differences: {string.Join("", differences)}");
    }
    while (!differences.All(n => n == 0));

    var extrapolated = lastNumbers[0];

    for (var i = 1; i < lastNumbers.Count; i++)
    {
        extrapolated += lastNumbers[i];
    }

    Console.WriteLine($"Extrapolated: {extrapolated}");

    total += extrapolated;

    Console.WriteLine();
}

Console.WriteLine($"Total: {total}");