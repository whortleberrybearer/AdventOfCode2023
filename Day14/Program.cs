var input = File.ReadLines("Input.txt").ToArray();
var panel = new char[input.Count()][];

for (var i = 0; i < input.Count(); i++)
{
    var line = input[i];
    
    Console.WriteLine(line);

    panel[i] = line.ToCharArray();
}

for (var x = 0; x < panel[0].Length; x++)
{
    for (var y = 1; y < panel.Length; y++)
    {
        if (panel[y][x] == 'O')
        {
            MoveUpIfAvailable(y, x);
        }
    }
}

var totalLoad = 0;

for (var i = 0; i < panel.Length; i++)
{
    var multiplier = panel.Length - i;

    var load = panel[i].Count(c => c == 'O') * multiplier;
    
    Console.WriteLine($"Line: {multiplier}, Load: {load}");

    totalLoad += load;
}

Console.WriteLine($"Total load: {totalLoad}");

void MoveUpIfAvailable(int y, int x)
{
    if (y == 0)
    {
        return;        
    }

    if (panel[y - 1][x] == '.')
    {
        panel[y - 1][x] = 'O';
        panel[y][x] = '.';
        
        MoveUpIfAvailable(y - 1, x);
    }
}