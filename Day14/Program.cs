var input = File.ReadLines("Input.txt").ToArray();
var panel = new char[input.Count()][];

for (var i = 0; i < input.Count(); i++)
{
    var line = input[i];
    
    Console.WriteLine(line);

    panel[i] = line.ToCharArray();
}

Console.WriteLine();

TiltPanelNorth();
TiltPanelWest();
TiltPanelSouth();
TiltPanelEast();

TiltPanelNorth();
TiltPanelWest();
TiltPanelSouth();
TiltPanelEast();

TiltPanelNorth();
TiltPanelWest();
TiltPanelSouth();
TiltPanelEast();

foreach (var line in panel)
{
    Console.WriteLine(new string(line));
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

void TiltPanelNorth()
{
    for (var x = 0; x < panel[0].Length; x++)
    {
        for (var y = 0; y < panel.Length; y++)
        {
            if (panel[y][x] == 'O')
            {
                MoveNorthIfAvailable(y, x);
            }
        }
    }
}

void TiltPanelEast()
{
    for (var x = panel[0].Length - 1; x >= 0; x--)
    {
        for (var y = 0; y < panel.Length; y++)
        {
            if (panel[y][x] == 'O')
            {
                MoveEastIfAvailable(y, x);
            }
        }
    }
}

void TiltPanelSouth()
{
    for (var x = 0; x < panel[0].Length; x++)
    {
        for (var y = panel.Length - 1; y >= 0; y--)
        {
            if (panel[y][x] == 'O')
            {
                MoveSouthIfAvailable(y, x);
            }
        }
    }
}

void TiltPanelWest()
{
    for (var x = 0; x < panel[0].Length; x++)
    {
        for (var y = 0; y < panel.Length; y++)
        {
            if (panel[y][x] == 'O')
            {
                MoveWestIfAvailable(y, x);
            }
        }
    }
}

void MoveNorthIfAvailable(int y, int x)
{
    if (y == 0)
    {
        return;        
    }

    if (panel[y - 1][x] == '.')
    {
        panel[y - 1][x] = 'O';
        panel[y][x] = '.';
        
        MoveNorthIfAvailable(y - 1, x);
    }
}

void MoveEastIfAvailable(int y, int x)
{
    if (x == (panel.Length - 1))
    {
        return;        
    }

    if (panel[y][x + 1] == '.')
    {
        panel[y][x + 1] = 'O';
        panel[y][x] = '.';
        
        MoveEastIfAvailable(y, x + 1);
    }
}

void MoveSouthIfAvailable(int y, int x)
{
    if (y == (panel.Length - 1))
    {
        return;        
    }

    if (panel[y + 1][x] == '.')
    {
        panel[y + 1][x] = 'O';
        panel[y][x] = '.';
        
        MoveSouthIfAvailable(y + 1, x);
    }
}


void MoveWestIfAvailable(int y, int x)
{
    if (x == 0)
    {
        return;        
    }

    if (panel[y][x - 1] == '.')
    {
        panel[y][x - 1] = 'O';
        panel[y][x] = '.';
        
        MoveWestIfAvailable(y, x - 1);
    }
}
