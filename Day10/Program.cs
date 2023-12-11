var input = File.ReadAllLines("Input.txt");
var maxtix = new char[input.Length][];
var route = new char[maxtix.Length * 3][];
int startX = 0;
int startY = 0;

for (var lineIndex = 0; lineIndex < input.Length; lineIndex++)
{
    var line = input[lineIndex];

    Console.WriteLine($"Line: {line}");

    maxtix[lineIndex] = line.ToCharArray();

    var s = line.IndexOf('S');

    if (s > -1)
    {
        startX = s;
        startY = lineIndex;

        Console.WriteLine($"StartX: {startX}, StartY: {startY}");
    }

    // Add the gaps to the route matrix as will be needing later.  // Anything on the edge can not be enclosed.
    var routeIndex = lineIndex * 3;

    if (lineIndex == 0)
    {
        route[routeIndex] = Enumerable.Repeat('0', line.Length * 3).ToArray();
        route[routeIndex + 1] = Enumerable.Repeat('.', line.Length * 3).ToArray();
        route[routeIndex + 2] = Enumerable.Repeat('.', line.Length * 3).ToArray();
        route[routeIndex + 1][0] = '0';
        route[routeIndex + 2][0] = '0';
        route[routeIndex + 1][(line.Length * 3) - 1] = '0';
        route[routeIndex + 2][(line.Length * 3) - 1] = '0';
    }
    else if (lineIndex == input.Length - 1)
    {
        route[routeIndex] = Enumerable.Repeat('.', line.Length * 3).ToArray();
        route[routeIndex + 1] = Enumerable.Repeat('.', line.Length * 3).ToArray();
        route[routeIndex + 2] = Enumerable.Repeat('0', line.Length * 3).ToArray();
        route[routeIndex][0] = '0';
        route[routeIndex + 1][0] = '0';
        route[routeIndex][(line.Length * 3) - 1] = '0';
        route[routeIndex + 1][(line.Length * 3) - 1] = '0';
    }
    else
    {
        // Random pipes can be enclosed, so set all to '.';
        route[routeIndex] = Enumerable.Repeat('.', line.Length * 3).ToArray();
        route[routeIndex + 1] = Enumerable.Repeat('.', line.Length * 3).ToArray();
        route[routeIndex + 2] = Enumerable.Repeat('.', line.Length * 3).ToArray();

        // The edges also can not be enclosed.
        route[routeIndex][0] = '0';
        route[routeIndex + 1][0] = '0';
        route[routeIndex + 2][0] = '0';
        route[routeIndex][(line.Length * 3) - 1] = '0';
        route[routeIndex + 1][(line.Length * 3) - 1] = '0';
        route[routeIndex + 2][(line.Length * 3) - 1] = '0';
    }
}

var steps = 0;
var currentPipe = 'S';
var currentX = startX;
var currentY = startY;
var currentDirection = FindStartDirection();

do
{
    ++steps;

    if (currentDirection == Direction.North)
    {
        --currentY;
    }
    else if (currentDirection == Direction.South)
    {
        ++currentY;
    }
    else if (currentDirection == Direction.East)
    {
        ++currentX;
    }
    else if (currentDirection == Direction.West)
    {
        --currentX;
    }

    currentPipe = maxtix[currentY][currentX];
    route[(currentY * 3) + 1][(currentX * 3) + 1] = currentPipe;

    if (currentPipe == '-')
    {
        route[(currentY * 3) + 1][currentX * 3] = '-';
        route[(currentY * 3) + 1][(currentX * 3) + 2] = '-';
    }
    else if (currentPipe == '|')
    {
        route[currentY * 3][(currentX * 3) + 1] = '|';
        route[(currentY * 3) + 2][(currentX * 3) + 1] = '|';
    }
    else if (currentPipe == '7')
    {
        route[(currentY * 3) + 1][currentX * 3] = '-';
        route[(currentY * 3) + 2][(currentX * 3) + 1] = '|';
    }
    else if (currentPipe == 'J')
    {
        route[currentY * 3][(currentX * 3) + 1] = '|';
        route[(currentY * 3) + 1][currentX * 3] = '-';
    }
    else if (currentPipe == 'L')
    {
        route[currentY * 3][(currentX * 3) + 1] = '|';
        route[(currentY * 3) + 1][(currentX * 3) + 2] = '-';
    }
    else if (currentPipe == 'F')
    {
        route[(currentY * 3) + 1][(currentX * 3) + 2] = '-';
        route[(currentY * 3) + 2][(currentX * 3) + 1] = '|';
    }

    if (currentPipe != 'S')
    {
        currentDirection = FindCurrentDirection(currentPipe, currentDirection);
    }
        
    Console.WriteLine($"Current pipe: {currentPipe}, Direction: {currentDirection}");
}
while (currentPipe != 'S');

Console.WriteLine($"Steps: {steps}");

var furthest = steps / 2;

Console.WriteLine($"Furthest: {furthest}");

// Replace the 'S' with a pipe.

if (route[(startY * 3) + 1][(startX * 3) + 3] == '-')
{
    route[(startY * 3) + 1][(startX * 3) + 2] = '-'; 
}

if (route[(startY * 3) + 1][(startX * 3) - 1] == '-')
{
    route[(startY * 3) + 1][(startX * 3)] = '-';
}

if (route[(startY * 3) + 3][(startX * 3) + 1] == '|')
{
    route[(startY * 3) + 2][(startX * 3) + 1] = '|';
}

if (startY > 0)
{
    if (route[(startY * 3) - 1][(startX * 3) + 1] == '|')
    {
        route[(startY * 3)][(startX * 3) + 1] = '|';
    }
}


// Mark anything not enclosed;
var replaced = false;

do
{
    replaced = false;

    for (var y = 1; y < route.Length - 1; y++)
    {
        for (var x = 1; x < route[y].Length - 1; x++)
        {
            if (route[y][x] == '.')
            {
                // If next to a 0, not enclosed.
                for (var i = y - 1; i <= y + 1; i++)
                {
                    for (var j = x - 1; j <= x + 1; j++)
                    {
                        if (route[i][j] == '0')
                        {
                            route[y][x] = '0';

                            replaced = true;
                        }
                    }
                }
            }
        }
    }
}
while (replaced);

// Now replace middle '.' is not enclosed.
for (var y = 1; y < route.Length; y += 3)
{
    for (var x = 1; x < route[y].Length; x += 3)
    {
        // If sounded by '.', its enclosed.
        var sounding = new char[]
        {
            route[y - 1][x - 1],
            route[y - 1][x],
            route[y - 1][x + 1],
            route[y][x - 1],
            route[y][x + 1],
            route[y + 1][x - 1],
            route[y + 1][x],
            route[y + 1][x + 1]
        };

        if (sounding.All(c => c == '.'))
        {
            route[y][x] = 'I';
        }
    }
}

for (var y = 1; y < route.Length; y += 3)
{
    for (var x = 1; x < route[y].Length; x += 3)
    {
        Console.Write(route[y][x]);
    }

    Console.WriteLine();
}
 
var enclosed = route.Sum(l => l.Count(c => c == 'I'));

Console.WriteLine($"Enclosed: {enclosed}");
Direction FindStartDirection()
{
    var east = startX < maxtix[startY].Length - 1 ? maxtix[startY][startX + 1] is '-' or 'J' or '7' : false;
    var west = startX > 0 ? maxtix[startY][startX - 1] is '-' or 'L' or 'F' : false;

    if (east)
    {
        return Direction.East;
    }
    else if (west)
    {
        return Direction.West;
    }

    return Direction.North;
}

Direction FindCurrentDirection(char pipe, Direction fromDirection)
{
    if (pipe == '-')
    {
        return fromDirection;
    }
    else if (pipe == '|')
    {
        return fromDirection;
    }
    else if (pipe == '7')
    {
        return fromDirection == Direction.East ? Direction.South : Direction.West;
    }
    else if (pipe == 'F')
    {
        return fromDirection == Direction.West ? Direction.South : Direction.East;
    }
    else if (pipe == 'J')
    {
        return fromDirection == Direction.East ? Direction.North : Direction.West;
    }
    else if (pipe == 'L')
    {
        return fromDirection == Direction.West ? Direction.North : Direction.East;
    }

    // Should never get here.
    throw new Exception();
}

enum Direction
{
    North,
    South,
    East,
    West,
}
