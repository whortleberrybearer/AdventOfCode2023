﻿using System.Numerics;

var input = File.ReadAllLines("Input.txt");
var bricks = new Dictionary<string, Brick>();
var id = "AAA";
var maxX = 0;
var maxY = 0;
var maxZ = 0;
var spacer = "...";

foreach (var line in input)
{
    Console.WriteLine(line);

    var parts = line.Split(',', '~').Select(c => int.Parse(c)).ToArray();
        
    bricks.Add(
        id,
        new Brick(
            id,    
            new Coordinate() { X = parts[0], Y = parts[1], Z = parts[2] },
            new Coordinate() { X = parts[3], Y = parts[4], Z = parts[5] }));

    maxX = Math.Max(maxX, Math.Max(parts[0], parts[3]));
    maxY = Math.Max(maxY, Math.Max(parts[1], parts[4]));
    maxZ = Math.Max(maxZ, Math.Max(parts[2], parts[5]));
    id = GetNextPrefix(id, "000");
}

Console.WriteLine();

maxX += 1;
maxY += 1;
maxZ += 1;

var stack = new string[maxX, maxY, maxZ];

foreach (var brick in bricks.Values)
{
    Console.WriteLine($"Placing brick: {brick.Id}");

    for (var x = brick.A.X; x <= brick.B.X; x++)
    {
        for (var y = brick.A.Y; y <= brick.B.Y; y++)
        {
            for (var z = brick.A.Z; z <= brick.B.Z; z++)
            {
                stack[x, y, z] = brick.Id;
            }
        }    
    }
}

Console.WriteLine();

// Descend the bricks
var repeat = true;

do
{
    repeat = false;

    foreach (var brick in bricks.Values)
    {
        if (MoveDown(brick))
        {
            repeat = true;
        }
    }
} 
while (repeat);

Console.WriteLine();
Console.WriteLine("X - Z Map");

for (var z = maxZ - 1; z >= 0 ; z--)
{
    for (var x = 0; x < maxX; x++)
    {
        var brickId = spacer;
        
        for (var y = 0; y < maxY; y++)
        {
            if (!string.IsNullOrEmpty(stack[x, y, z]))
            {
                if (brickId == spacer)
                {
                    brickId = stack[x, y, z];
                }
                else if (brickId != stack[x, y, z])
                {
                    brickId = "???";
                    break;
                }
            }
        }

        Console.Write(brickId);
    }    
    
    Console.WriteLine();
}

Console.WriteLine();
Console.WriteLine("Y - Z Map");

for (var z = maxZ - 1; z >= 0 ; z--)
{
    for (var y = 0; y < maxY; y++)
    {
        var brickId = spacer;
        
        for (var x = 0; x < maxX; x++)
        {
            if (!string.IsNullOrEmpty(stack[x, y, z]))
            {
                if (brickId == spacer)
                {
                    brickId = stack[x, y, z];
                }
                else if (brickId != stack[x, y, z])
                {
                    brickId = "???";
                    break;
                }
            }
        }

        Console.Write(brickId);
    }
    
    Console.WriteLine();
}

Console.WriteLine();

var bricksPossibleToRemove = new List<Brick>();
var fallingBricks = new Dictionary<Brick, int>();

foreach (var brick in bricks.Values)
{
    // Find any bricks above it.
    var bricksAbove = FindBricksAbove(brick);
    var canRemove = true;
    
    foreach (var brickAbove in bricksAbove)
    {
        if (WillBrickFall(brickAbove, new string[] { brick.Id }))
        {
            canRemove = false;

            break;
        }
    }

    if (canRemove)
    {
        bricksPossibleToRemove.Add(brick);

        Console.WriteLine($"Brick: {brick.Id} can be removed");
    }
    else
    {
        fallingBricks.Add(brick, 0);
    }
}

Console.WriteLine($"Total can remove: {bricksPossibleToRemove.Count()}");
Console.WriteLine();

foreach (var brick in fallingBricks.Keys.ToArray())
{
    var falling = CalculateFallingBricks(brick, new List<Brick>() { brick });

    fallingBricks[brick] = falling.Count();
    
    Console.WriteLine($"Brick: {brick.Id}, Falling: {falling.Count()}");
}

Console.WriteLine($"Total falling bricks: {fallingBricks.Values.Sum()}");

IEnumerable<Brick> CalculateFallingBricks(Brick brick, List<Brick> bricksBelow)
{
    var bricksAbove = FindBricksAbove(brick);
    var bricksThatWillFall = new List<Brick>();

    foreach (var brickAbove in bricksAbove)
    {
        if (WillBrickFall(brickAbove, bricksBelow.Select(b => b.Id)))
        {
            bricksBelow.Add(brickAbove);
            bricksThatWillFall.Add(brickAbove);
            
            bricksThatWillFall.AddRange(CalculateFallingBricks(brickAbove, bricksBelow));
        }
    }

    return bricksThatWillFall.Distinct();
}

bool WillBrickFall(Brick brick, IEnumerable<string> idsBelow)
{
    for (var x = brick.A.X; x <= brick.B.X; x++)
    {
        for (var y = brick.A.Y; y <= brick.B.Y; y++)
        {
            for (var z = brick.A.Z; z <= brick.B.Z; z++)
            {
                if (!string.IsNullOrEmpty(stack[x, y, z - 1]) && (stack[x, y, z - 1] != brick.Id) && (!idsBelow.Contains(stack[x, y, z - 1])))
                {
                    // There is a different brick under this one.
                    return false;
                }
            }
        }
    }

    return true;
}

IEnumerable<Brick> FindBricksAbove(Brick brick)
{
    var bricksAbove = new List<Brick>();

    for (var x = brick.A.X; x <= brick.B.X; x++)
    {
        for (var y = brick.A.Y; y <= brick.B.Y; y++)
        {
            for (var z = brick.A.Z; z <= brick.B.Z; z++)
            {
                if (!string.IsNullOrEmpty(stack[x, y, z + 1]) && (stack[x, y, z + 1] != brick.Id))
                {
                    bricksAbove.Add(bricks[stack[x, y, z + 1]]);
                }
            }
        }
    }

    return bricksAbove.Distinct();
}

bool MoveDown(Brick brick)
{
    if (Math.Min(brick.A.Z, brick.B.Z) > 1)
    {
        var canMove = true;
        
        for (var x = brick.A.X; x <= brick.B.X; x++)
        {
            for (var y = brick.A.Y; y <= brick.B.Y; y++)
            {
                for (var z = brick.A.Z; z <= brick.B.Z; z++)
                {
                    var below = stack[x, y, z - 1];

                    if (!string.IsNullOrEmpty(below) && below != brick.Id)
                    {
                        canMove = false;
                        break;
                    }
                }
            }
        }

        if (canMove)
        {
            Console.WriteLine($"Moving brick: {brick.Id} down");
            
            for (var x = brick.A.X; x <= brick.B.X; x++)
            {
                for (var y = brick.A.Y; y <= brick.B.Y; y++)
                {
                    for (var z = brick.A.Z; z <= brick.B.Z; z++)
                    {
                        stack[x, y, z] = string.Empty;
                        stack[x, y, z - 1] = brick.Id;   
                    }
                }
            }

            brick.A.Z -= 1;
            brick.B.Z -= 1;

            // Check if can move down again.  Not the most efficient way of doing it.
            MoveDown(brick);

            return true;
        }
    }

    return false;
}

// Taken from https://stackoverflow.com/questions/23896355/increment-string-value-from-aaa-to-aab-to-aac-and-so-on.
string GetNextPrefix(string prefix, string format = "000")
{
    const string Charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    //remove the ToUpper() if you want to expand the charset to contains both cases
    var values = prefix.ToUpper().Select(c => Charset.IndexOf(c));
    if (values.Any(v => v == -1))
        throw new Exception("Invalid Prefix : " + prefix);

    var value = values.Aggregate(0, (acc,v) => acc = acc * Charset.Length + v) + 1;
    var result = String.Empty;

    while(value != 0)
    {
        var remainder = value % Charset.Length;

        result = Charset[remainder] + result;
        value = (value - remainder) / Charset.Length;
    }

    return result.PadLeft(format.Length, Charset[0]);
}

record Coordinate
{
    public int X { get; set; }
    
    public int Y { get; set; }

    public int Z { get; set; }
}

record Brick(string Id, Coordinate A, Coordinate B);