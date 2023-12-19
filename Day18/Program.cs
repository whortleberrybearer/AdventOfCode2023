var input = File.ReadAllLines("Input.txt");
var planLines = new List<Plan>();

foreach (var line in input)
{
    Console.WriteLine(line);

    var parts = line.Split(' ');

    planLines.Add(new Plan(parts[0], int.Parse(parts[1]), parts[2].Trim('(', ')', '#')));
}

Console.WriteLine();

/*
// Recode the lines.
var newPlanLines = new List<Plan>();

foreach (var planLine in planLines)
{
    var direction = planLine.Colour[5] switch
    {
        '0' => "R",
        '1' => "D",
        '2' => "L",
        '3' => "U",
    };
    var amount = int.Parse(planLine.Colour[0..5], System.Globalization.NumberStyles.HexNumber);
    
    newPlanLines.Add(new Plan(direction, amount, string.Empty));
}

planLines = newPlanLines;
*/
// Dont know the size of the grid, but a theoretical size is the max right and down movements.
var width = planLines.Where(l => l.Direction == "R").Sum(l => l.Amount + 1) + planLines.Where(l => l.Direction == "L").Sum(l => l.Amount + 1);
var height = planLines.Where(l => l.Direction == "D").Sum(l => l.Amount + 1) + planLines.Where(l => l.Direction == "U").Sum(l => l.Amount + 1);

width /= 100;
height /= 100;

var plan = new int[height][];

for (var i = 0; i < plan.Length; i++)
{
    plan[i] = Enumerable.Repeat(-1, width).ToArray();
}

var x = width / 2;
var y = height / 2;
var xVal = 100;
var yVal = 100;

foreach (var planLine in planLines)
{
    Console.WriteLine($"Direction: {planLine.Direction}, Amount: {planLine.Amount}");
    
    for (var i = 0; i < planLine.Amount / 100; i++)
    {
        if (planLine.Direction == "R")
        {
            plan[y][x] = 100 * yVal;
            ++x;
        }
        else if (planLine.Direction == "L")
        {
            plan[y][x] = 100 * yVal;
            --x;
        }
        else if (planLine.Direction == "U")
        {
            plan[y][x] = 100 * xVal;
            --y;
        }
        else if (planLine.Direction == "D")
        {
            plan[y][x] = 100 * xVal;
            ++y;
        }
    }
    
    if (planLine.Direction == "R")
    {
        xVal = planLine.Amount % 100;
        plan[y][x] = xVal * yVal;
    }
    else if (planLine.Direction == "L")
    {
        xVal = planLine.Amount % 100;
        plan[y][x] = xVal * yVal;
    }
    else if (planLine.Direction == "U")
    {
        yVal = planLine.Amount % 100;
        plan[y][x] = xVal * yVal;
    }
    else if (planLine.Direction == "D")
    {
        yVal = planLine.Amount % 100;
        plan[y][x] = xVal * yVal;
    }
}

Console.WriteLine("Filling");

// Mark anything not enclosed;
for (var i = 0; i < plan.Length; i++)
{
    if (plan[i][0] == -1)
    {
        plan[i][0] = 0;
    }
    
    if (plan[i][plan[y].Length - 1] == -1)
    {
        plan[i][plan[y].Length - 1] = 0;
    }
}

for (var i = 0; i < plan[0].Length; i++)
{
    if (plan[0][i] == -1)
    {
        plan[0][i] = 0;
    }
    
    if (plan[plan.Length - 1][i] == -1)
    {
        plan[plan.Length - 1][i] = 0;
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
            if (plan[y][x] == -1)
            {
                // If next to a 0, not enclosed.
                for (var i = y - 1; i <= y + 1; i++)
                {
                    for (var j = x - 1; j <= x + 1; j++)
                    {
                        if (plan[i][j] == 0)
                        {
                            plan[y][x] = 0;

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

var digArea = plan.Sum(p => p.LongCount(c => c != 0));

Console.WriteLine($"Dig area: {digArea}");

record Plan(string Direction, int Amount, string Colour);