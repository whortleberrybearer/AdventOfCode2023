var input = File.ReadAllText("Input.txt");
var total = 0;
var races = new List<Race>();
int[] times = null;
int[] distances = null;
var possibleWins = new List<int>();

foreach (var line in input.Split("\r\n"))
{
    if (line.StartsWith("Time:"))
    {
        times = line.Replace("Time:", string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
    }
    else if (line.StartsWith("Distance:"))
    {
        distances = line.Replace("Distance:", string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
    }
}

for (int i = 0; i < times!.Count(); i++)
{
    var maxTime = times![i];
    var maxDistance = distances![i];

    Console.WriteLine($"Race Time: {maxTime}, Distance: {maxDistance}");

    var wins = 0;

    for (int time = 1; time < maxTime; time++)
    {
        var distance = time * (maxTime - time);

        Console.WriteLine($"Press Time: {time}, Distance: {distance}");

        if (distance > maxDistance)
        {
            ++wins;
        }
    }

    possibleWins.Add(wins);

    Console.WriteLine($"Possible wins: {wins}");

    Console.WriteLine();
}

total = 1;

foreach (int value in possibleWins)
{
    total *= value;
}

Console.WriteLine($"Total: {total}");

record Race(int Time, int Distance);