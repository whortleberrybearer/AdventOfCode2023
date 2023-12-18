var input = File.ReadAllLines("Input.txt");
var planLines = new List<Plan>();

foreach (var line in input)
{
    Console.WriteLine(line);

    var parts = line.Split(' ');

    planLines.Add(new Plan(parts[0], int.Parse(parts[1]), parts[2]));
}

Console.WriteLine();

// Dont know the size of the grid, but a theoretical size is the max right and down movements.
var width = planLines.Where(l => l.Direction == "R").Sum(l => l.Amount + 1) + planLines.Where(l => l.Direction == "L").Sum(l => l.Amount + 1);
var height = planLines.Where(l => l.Direction == "D").Sum(l => l.Amount + 1) + planLines.Where(l => l.Direction == "U").Sum(l => l.Amount + 1);

var plan = new char[height][];

for (var i = 0; i < plan.Length; i++)
{
    plan[i] = Enumerable.Repeat('.', width).ToArray();
}

var x = width / 2;
var y = height / 2;

foreach (var planLine in planLines)
{
    for (var i = 0; i < planLine.Amount; i++)
    {
        plan[y][x] = '#';
        
        if (planLine.Direction == "R")
        {
            ++x;
        }
        else if (planLine.Direction == "L")
        {
            --x;
        }
        else if (planLine.Direction == "U")
        {
            --y;
        }
        else if (planLine.Direction == "D")
        {
            ++y;
        }
    }
}

// Mark anything not enclosed;
for (var i = 0; i < plan.Length; i++)
{
    if (plan[i][0] == '.')
    {
        plan[i][0] = '0';
    }
    
    if (plan[i][plan[y].Length - 1] == '.')
    {
        plan[i][plan[y].Length - 1] = '0';
    }
}

for (var i = 0; i < plan[0].Length; i++)
{
    if (plan[0][i] == '.')
    {
        plan[0][i] = '0';
    }
    
    if (plan[plan.Length - 1][i] == '.')
    {
        plan[plan.Length - 1][i] = '0';
    }
}

var replaced = false;

do
{
    replaced = false;

    for (y = 1; y < plan.Length - 1; y++)
    {
        for (x = 1; x < plan[y].Length - 1; x++)
        {
            if (plan[y][x] == '.')
            {
                // If next to a 0, not enclosed.
                for (var i = y - 1; i <= y + 1; i++)
                {
                    for (var j = x - 1; j <= x + 1; j++)
                    {
                        if (plan[i][j] == '0')
                        {
                            plan[y][x] = '0';

                            replaced = true;
                        }
                    }
                }
            }
        }
    }
}
while (replaced);

foreach (var line in plan)
{
    Console.WriteLine(string.Concat(line));
}

var digArea = plan.Sum(p => p.Count(c => c == '.' || c == '#'));

Console.WriteLine($"Dig area: {digArea}");

record Plan(string Direction, int Amount, string Colour);