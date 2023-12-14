var input = File.ReadLines("Input.txt").ToArray();
var panel = new char[input.Count()][];
var rotationsHash = new List<string>();

for (var i = 0; i < input.Count(); i++)
{
    var line = input[i];
    
    Console.WriteLine(line);

    panel[i] = line.ToCharArray();
}

Console.WriteLine();
var repeatIndex = -1;

while ((rotationsHash.LongCount() < 1000000000l) && (repeatIndex == -1))
{
    TiltPanelNorth();
    TiltPanelWest();
    TiltPanelSouth();
    TiltPanelEast();

    var hash = string.Join("|", panel.Select(l => new string(l)));

    repeatIndex = rotationsHash.IndexOf(hash);
    
    if (repeatIndex == -1)
    {
        rotationsHash.Add(hash);
    }
}

var offset = ((1000000000 - (repeatIndex + 1)) % (rotationsHash.Count() - repeatIndex)) + repeatIndex;

var totalLoad = CalculateTotalLoad(rotationsHash[offset].Split("|").Select(l => l.ToCharArray()).ToArray());

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

int CalculateTotalLoad(char[][] panel)
{
    var totalLoad = 0;
    
    for (var i = 0; i < panel.Length; i++)
    {
        var multiplier = panel.Length - i;

        var load = panel[i].Count(c => c == 'O') * multiplier;
    
        Console.WriteLine($"Line: {multiplier}, Load: {load}");

        totalLoad += load;
    }

    return totalLoad;
}