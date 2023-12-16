﻿var input = File.ReadAllLines("Input.txt");
var grid = new char[input.Length][];
var energizedGrid = new string[input.Length][];

for (var i = 0; i < input.Length; i++)
{
    var line = input[i];

    Console.WriteLine(line);

    grid[i] = input[i].ToCharArray();
    energizedGrid[i] = Enumerable.Repeat(string.Empty, grid[i].Length).ToArray();
}

Move(0, 0, 1, 0);

Console.WriteLine();

foreach (var line in energizedGrid)
{
    foreach (var s in line)
    {
        Console.Write(s == string.Empty ? "." : "#");
    }

    Console.WriteLine();
}

var energized = energizedGrid.Sum(l => l.Count(c => c != string.Empty));

Console.WriteLine($"Energized: {energized}");

void Move(int x, int y, int moveX, int moveY)
{
    if ((x < 0) || (y < 0) || (x >= grid[0].Length) || (y >= grid.Length))
    {
        return;
    }

    if (moveX < 0)
    {
        if (energizedGrid[y][x].Contains("W"))
        {
            return;
        }    
        else
        {
            energizedGrid[y][x] += "W";
        }
    }
    else if (moveX > 0)
    {
        if (energizedGrid[y][x].Contains("E"))
        {
            return;
        }
        else
        {
            energizedGrid[y][x] += "E";
        }
    }
    else if (moveY < 0)
    {
        if (energizedGrid[y][x].Contains("N"))
        {
            return;
        }
        else
        {
            energizedGrid[y][x] += "N";
        }
    }
    else if (moveY > 0)
    {
        if (energizedGrid[y][x].Contains("S"))
        {
            return;
        }
        else
        {
            energizedGrid[y][x] += "S";
        }
    }

    var item = grid[y][x];

    if (item == '.')
    {
        Move(x + moveX, y + moveY, moveX, moveY);
    }
    else if (item == '|')
    {
        // If moving vertically, pass through, otherwise split;
        if (moveY != 0)
        {
            Move(x + moveX, y + moveY, moveX, moveY);
        }
        else
        {
            Move(x, y + 1, 0, 1);
            Move(x, y - 1, 0, -1);
        }
    }
    else if (item == '-')
    {
        // If moving horizontally, pass through, otherwise split;
        if (moveX != 0)
        {
            Move(x + moveX, y + moveY, moveX, moveY);
        }
        else
        {
            Move(x + 1, y, 1, 0);
            Move(x - 1, y, -1, 0);
        }
    }
    else if (item == '/')
    {
        Move(x + (moveY * -1), y + (moveX * -1), moveY * -1, moveX * -1);
    }
    else if (item == '\\')
    {
        Move(x + moveY, y + moveX, moveY, moveX);
    }
}


