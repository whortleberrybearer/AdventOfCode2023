var input = File.ReadAllLines("Input.txt");
var map = input.Select(l => l.ToCharArray()).ToArray();
var visited = new char[map.Length, map[0].Length];
var stepsTaken = new StepsTaken[map.Length, map[0].Length];
var startX = -1;
var startY = -1;
var numberOfSteps = 64;

for (var y = 0; y < map.Length; y++)
{
    for (var x = 0; x < map[y].Length; x++)
    {
        if (map[y][x] == 'S')
        {
            startX = x;
            startY = y;
            break;
        }
    }
}

var movements = new List<Movement>() { new Movement(startX, startY, numberOfSteps) };

do
{
    var toMove = movements.ToArray();
    movements.Clear();

    foreach (var movement in toMove)
    {
        movements.AddRange(CheckPlots(movement));
    }
} 
while (movements.Any());

Console.WriteLine();

var total = 0;

if ((numberOfSteps % 2) == 0)
{
    // Add 1 for the S
    total += 1;
}

for (var y = 0; y < map.Length; y++)
{
    for (var x = 0; x < map[y].Length; x++)
    {
        if (((numberOfSteps % 2) == 0) && stepsTaken[y, x].Even)
        {
            ++total;
            Console.Write('0');    
        }
        else if (((numberOfSteps % 2) == 1) && stepsTaken[y, x].Even)
        {
            ++total;
            Console.Write('0');    
        }
        else
        {
            Console.Write(map[y][x]);
        }
    }

    Console.WriteLine();
}

Console.WriteLine($"Total gardens: {total}");

IEnumerable<Movement> CheckPlots(Movement movement)
{
    var newMovements = new List<Movement>();

    var newMovement = CheckMovement(movement.X - 1, movement.Y, movement.MovementsRemaining);

    if (newMovement != null)
    {
        newMovements.Add(newMovement);
    }
    
    newMovement = CheckMovement(movement.X + 1, movement.Y, movement.MovementsRemaining);

    if (newMovement != null)
    {
        newMovements.Add(newMovement);
    }
    
    newMovement = CheckMovement(movement.X, movement.Y - 1, movement.MovementsRemaining);

    if (newMovement != null)
    {
        newMovements.Add(newMovement);
    }
    
    newMovement = CheckMovement(movement.X, movement.Y + 1, movement.MovementsRemaining);

    if (newMovement != null)
    {
        newMovements.Add(newMovement);
    }

    return newMovements;
}

Movement? CheckMovement(int x, int y, int movementsRemaining)
{
    if (CanMove(x, y))
    {
        Console.WriteLine($"Moved to X: {x}, Y: {y}");

        visited[y, x] = '0';
        stepsTaken[y, x].Count = movementsRemaining;

        if (movementsRemaining > 1)
        {
            return new Movement(x, y, movementsRemaining - 1);
        }
    }

    return null;
}

bool CanMove(int x, int y)
{
    if (x < 0 || y < 0)
    {
        return false;
    }

    if ((x >= map[0].Length) || (y >= map.Length))
    {
        return false;
    }

    return map[y][x] == '.' && visited[y, x] != '0';
}

record Movement(int X, int Y, int MovementsRemaining);

struct StepsTaken
{
    public bool Even => Count % 2 == 1;

    public bool Odd => Count % 2 == 1;
    
    public int Count { get; set; }
}