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

    if (mirror == -1)
    {
        mirror = FindColumnMirror(map.Locations);

        if (mirror > -1)
        {
            map.Column = mirror + 1;

            Console.WriteLine($"Column mirror at: {map.Column}");
        }
    }

    Console.WriteLine();
}

var total = maps.Sum(m => m.Row) + maps.Sum(m => m.Column * 100);

Console.WriteLine($"Total: {total}");

int FindRowMirror(char[][] map)
{
    for (var i = 0; i < map[0].Length - 1; i++)
    {
        var reflection = true;
        var canSmudge = true;

        for (var j = 0; j < map.Length; j++)
        {
            if (!CompareReflection(map[j], i, ref canSmudge))
            {
                reflection = false;
                break;
            }
        }

        // Ignore any reflections that have not been smudged.
        if (reflection && !canSmudge)
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
        var canSmudge = true;

        // Can treat a column line a row.
        for (int j = 0; j < map[0].Length; j++)
        {
            var column = map.Select(r => r[j]).ToArray();

            if (!CompareReflection(column, i, ref canSmudge))
            {
                reflection = false;
                break;
            }
        }

        // Ignore any reflections that have not been smudged.
        if (reflection && !canSmudge)
        {
            return i;
        }
    }

    return -1;
}

bool CompareReflection(char[] line, int position, ref bool canSmudge)
{
    var reflectBack = position;
    var reflectForward = position + 1;

    while ((reflectBack >= 0) && (reflectForward < line.Length))
    {
        if (line[reflectBack] != line[reflectForward])
        {
            // Not a matching row.  If can smudge, allow to continue as it has changed.
            if (canSmudge)
            {
                canSmudge = false;
            }
            else
            {
                return false;   
            }
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