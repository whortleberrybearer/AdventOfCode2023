var input = File.ReadAllText("Input.txt");
var total = 0l;
var races = new List<Race>();
long[] times = null;
long[] distances = null;
var possibleWins = new List<long>();

foreach (var line in input.Split("\r\n"))
{
    if (line.StartsWith("Time:"))
    {
        times = line.Replace("Time:", string.Empty).Replace(" ", string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => long.Parse(n)).ToArray();
    }
    else if (line.StartsWith("Distance:"))
    {
        distances = line.Replace("Distance:", string.Empty).Replace(" ", string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => long.Parse(n)).ToArray();
    }
}

for (int i = 0; i < times!.Count(); i++)
{
    var maxTime = times![i];
    var maxDistance = distances![i];

    Console.WriteLine($"Race Time: {maxTime}, Distance: {maxDistance}");

    var minWinTime = 0l;

    // Could divide way though this, but just go for the inefficient process.
    for (long time = 1; time < maxTime; time++)
    {
        var distance = time * (maxTime - time);

        Console.WriteLine($"Press Time: {time}, Distance: {distance}");

        if (distance > maxDistance)
        {
            minWinTime = time;

            break;
        }
    }

    var wins = maxTime - (minWinTime * 2) + 1;

    possibleWins.Add(wins);

    Console.WriteLine($"Possible wins: {wins}");

    Console.WriteLine();
}

total = 1;

foreach (var value in possibleWins)
{
    total *= value;
}

Console.WriteLine($"Total: {total}");

record Race(int Time, int Distance);