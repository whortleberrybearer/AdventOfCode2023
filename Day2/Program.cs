var input = File.ReadAllText("Input.txt");
var total = 0;
var totalPower = 0;
var maxRed = 12;
var maxGreen = 13;
var maxBlue = 14;


foreach (var line in input.Split("\r\n"))
{
    Console.WriteLine($"Line: {line}");

    var parts = line.Split(':', ';', ',');
    int id = 0;
    int red = 0;
    int blue = 0;
    int green = 0;

    foreach (var part in parts)
    {
        if (part.StartsWith("Game "))
        {
            id = int.Parse(part.Replace("Game ", string.Empty));
        }
        else if (part.EndsWith(" red"))
        {
            var newRed = int.Parse(part.Replace(" red", string.Empty));

            if (newRed > red)
            {
                red = newRed;
            }
        }
        else if (part.EndsWith(" green"))
        {
            var newGreen = int.Parse(part.Replace(" green", string.Empty));

            if (newGreen > green)
            {
                green = newGreen;
            }
        }
        else if (part.EndsWith(" blue"))
        {
            var newBlue = int.Parse(part.Replace(" blue", string.Empty));

            if (newBlue > blue)
            {
                blue = newBlue;
            }
        }
    }

    Console.WriteLine($"Id: {id}");
    Console.WriteLine($"Red: {red}");
    Console.WriteLine($"Green: {green}");
    Console.WriteLine($"Blue: {blue}");

    if ((red <= maxRed) && (green <= maxGreen) && (blue <= maxBlue))
    {
        Console.WriteLine("Game is valid");

        // Part 1 needs the sum of the Ids.
        total += id;
    }
    else
    {
        Console.WriteLine("Game is invalid");
    }

    // Part 2 needs the power.
    var power = red * green * blue;
    totalPower += power;

    Console.WriteLine($"Power: {power}");

    Console.WriteLine();
}

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Total power: {totalPower}");