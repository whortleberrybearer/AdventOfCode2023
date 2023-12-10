var input = File.ReadAllLines("Input.txt");
var maxtix = new char[input.Length][];
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



/*
char FindCurrentDirection()
{
    var north = maxtix[startY - 1][startX] is '|' or '7' or 'F';
    var south = maxtix[startY + 1][startX] is '|' or 'J' or 'L';
    var east = maxtix[startY][startX + 1] is '-' or 'J' or '7';
    var west = maxtix[startY][startX - 1] is '-' or 'L' or 'F';

    if (north && south)
    {
        return '|';
    }
    else if (east && west)
    {
        return '-';
    }
    else if (north && east) 
    {
        return 'L';
    }
    else if (north && west)
    {
        return 'J';
    }
    else if (south && east)
    {
        return 'F';
    }
    else if (south && west)
    {
        return '7';
    }

    // Should never happen.
    return '.';
}
*/