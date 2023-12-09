var input = File.ReadAllText("Input.txt");
var totalFirst = 0;
var totalLast = 0;

foreach (var line in input.Split("\r\n"))
{
    Console.WriteLine($"Line: {line}");

    var parts = line.Split(' ').Select(c => int.Parse(c)).ToList();
    var differences = parts.ToList();
    var lastNumbers = new List<int>();
    var firstNumbers = new List<int>();

    do
    {
        firstNumbers.Insert(0, differences.First());
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

    var extrapolatedFirst = firstNumbers[0];
    var extrapolatedLast = lastNumbers[0];

    for (var i = 1; i < firstNumbers.Count; i++)
    {
        extrapolatedFirst = firstNumbers[i] - extrapolatedFirst;
    }

    for (var i = 1; i < lastNumbers.Count; i++)
    {
        extrapolatedLast += lastNumbers[i];
    }

    Console.WriteLine($"Extrapolated first: {extrapolatedFirst}");
    Console.WriteLine($"Extrapolated last: {extrapolatedLast}");

    totalFirst += extrapolatedFirst;
    totalLast += extrapolatedLast;

    Console.WriteLine();
}

Console.WriteLine($"Total first: {totalFirst}");
Console.WriteLine($"Total last: {totalLast}");