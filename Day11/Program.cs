var input = File.ReadLines("Input.txt").ToList();

// Add horizontal space.
for (var i = 0; i < input[0].Count(); i++)
{
    if (!input.Any(l => l[i] == '#'))
    {
        // Insert space into all lines.
        for (var j = 0; j < input.Count; j++)
        {
            input[j] = input[j].Insert(i, ".");
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
        input.Insert(i, input[i]);

        // Dont need to process blank line.
        i++;
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
var totalDistance = 0;

foreach (var pair in pairs)
{
    var distance = pair.ElementAt(0).DistanceTo(pair.ElementAt(1));

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
    public int DistanceTo(Galaxy other)
    {
        var xDiff = Math.Abs(X - other.X);
        var yDiff = Math.Abs(Y - other.Y);

        return xDiff + yDiff;
    }
}