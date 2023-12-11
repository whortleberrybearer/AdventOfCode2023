var input = File.ReadLines("Input.txt").ToList();

// Add horizontal space.
for (var i = 0; i < input[0].Count(); i++)
{
    if (!input.Any(l => l[i] == '#'))
    {
        // Insert space into all lines.
        for (var j = 0; j < input.Count; j++)
        {
            var line = input[j].ToCharArray();
            line[i] = '|';
            
            input[j] = new string(line);
        }

        i++;
    }
}

// Add vertical space.
for (var i = 0; i < input.Count; i++)
{
    if (!input[i].Contains('#'))
    {
        // No galaxies, add a new line.
        input[i] = input[i].Replace('.', '-');
    }
}

foreach (var line in input)
{
    Console.WriteLine(line);
}

// Find the galaxies.
var galaxies = new List<Galaxy>();

for (var y = 0; y < input.Count; y++)
{
    for (var x = 0; x < input[y].Length; x++)
    {
        if (input[y][x] == '#')
        {
            var galaxy = new Galaxy(galaxies.Count + 1, x, y);
            
            galaxies.Add(galaxy);
            
            Console.WriteLine($"Found galaxy: {galaxy}");
        }
    }
}

var pairs = GetPermutations(galaxies, 2);
var totalDistance = 0l;
var space = input.Select(l => l.ToCharArray()).ToArray();

foreach (var pair in pairs)
{
    var distance = pair.ElementAt(0).DistanceTo(pair.ElementAt(1), space);

    totalDistance += distance;
    
    Console.WriteLine($"Distance between {pair.ElementAt(0).Id} and {pair.ElementAt(1).Id}: {distance}");
}

Console.WriteLine($"Total distance: {totalDistance}");

// From https://stackoverflow.com/questions/12249051/unique-combinations-of-list.
IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
{
    int i = 0;
    foreach(var item in items)
    {
        if(count == 1)
            yield return new T[] { item };
        else
        {
            foreach(var result in GetPermutations(items.Skip(i + 1), count - 1))
                yield return new T[] { item }.Concat(result).ToArray();
        }

        ++i;
    }
}

record Galaxy(int Id, int X, int Y)
{
    private const int SpaceAge = 1000000;

    public int DistanceTo(Galaxy other, char[][] space)
    {
        var xMin = Math.Min(X, other.X);
        var xMax = Math.Max(X, other.X);
        var yMin = Math.Min(Y, other.Y);
        var yMax = Math.Max(Y, other.Y);

        var xSpace = 0;
        var ySpace = 0;

        for (var x = xMin + 1; x < xMax; x++)
        {
            if (space[0][x] == '|')
            {
                ++xSpace;
            }
        }

        for (var y = yMin + 1; y < yMax; y++)
        {
            if (space[y][0] == '-')
            {
                ++ySpace;
            }
        }

        return (xMax - xMin - xSpace) + (yMax - yMin - ySpace) + (xSpace * SpaceAge) + (ySpace * SpaceAge);
    }
}