var input = File.ReadLines("Input.txt");
var maps = new List<Map>();
var mapLines = new List<string>();

foreach (var line in input)
{
    Console.WriteLine(line);
    
    if (!string.IsNullOrWhiteSpace(line))
    {
        mapLines.Add(line);
    }
    else
    {
        maps.Add(new Map(mapLines.Select(l => l.ToCharArray()).ToArray()));
        mapLines.Clear();
    }
}

if (mapLines.Count > 0)
{
    maps.Add(new Map(mapLines.Select(l => l.ToCharArray()).ToArray()));
}

Console.WriteLine();

foreach (var map in maps)
{
    foreach (var line in map.Locations)
    {
        Console.WriteLine(line);
    }
    
    var mirror = FindRowMirror(map.Locations);

    if (mirror > -1)
    {
        map.Row = mirror + 1;

        Console.WriteLine($"Row mirror at: {map.Row}");
    }
    
    mirror = FindColumnMirror(map.Locations);

    if (mirror > -1)
    {
        map.Column = mirror + 1;

        Console.WriteLine($"Column mirror at: {map.Column}");
    }

    if ((map.Column == 0) && (map.Row == 0))
    {
        var i = 0;
    }
    
    Console.WriteLine();
}

var total = maps.Sum(m => m.Row) + maps.Sum(m => m.Column * 100);

Console.WriteLine($"Total: {total}");

int FindRowMirror(char[][] map)
{
    for (var i = 0; i < map[0].Length - 1; i++)
    {
        if (map.All(r => CompareReflection(r, i)))
        {
            return i;
        }
    }

    return -1;
}

int FindColumnMirror(char[][] map)
{
    for (var i = 0; i < map.Length - 1; i++)
    {
        var reflection = true;

        // Can treat a column line a row.
        for (int j = 0; j < map[0].Length; j++)
        {
            var column = map.Select(r => r[j]).ToArray();

            if (!CompareReflection(column, i))
            {
                reflection = false;
                break;
            }
        }

        if (reflection)
        {
            return i;
        }
    }

    return -1;
}

bool CompareReflection(char[] line, int position)
{
    var reflectBack = position;
    var reflectForward = position + 1;

    while ((reflectBack >= 0) && (reflectForward < line.Length))
    {
        if (line[reflectBack] != line[reflectForward])
        {
            // Not a matching row.
            return false;
        }

        // Move out to the next reflection.
        --reflectBack;
        ++reflectForward;
    }

    return true;
}

record Map(char[][] Locations)
{
    public int Row { get; set; }
    
    public int Column { get; set; }
};


// 32414 == too low
