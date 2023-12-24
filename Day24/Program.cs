using System.Drawing;
using System.Numerics;

var input = File.ReadAllLines("Input.txt");
var hailStones = new Dictionary<string, HailStone>();
var id = "AAA";

foreach (var line in input)
{
    Console.WriteLine(line);

    var parts = line.Split('@', ',').Select(c => long.Parse(c)).ToArray();

    hailStones.Add(
        id,
        new HailStone(
            id,
            new Vector2() { X = parts[0], Y = parts[1] },
            new Vector2() { X = parts[3], Y = parts[4] }));
    //new Vector2() { X = parts[0], Y = parts[1], Z = parts[2] },
    //new Vector2() { X = parts[3], Y = parts[4], Z = parts[5] }));

    id = GetNextPrefix(id, "000");
}

Console.WriteLine();

var textAreaMin = 200000000000000;
var testAreaMax = 400000000000000;

var totalIntersections = 0;

for (var i = 0; i < hailStones.Count; i++)
{
    for (var j = i + 1; j < hailStones.Count; j++)
    {
        var hailstone1 = hailStones.ElementAt(i).Value;
        var hailstone2 = hailStones.ElementAt(j).Value;

        var doesIntersect = TryFindIntersection(
            hailstone1.Position,
            hailstone1.Position + (hailstone1.Direction * 1000000000000000),
            hailstone2.Position,
            hailstone2.Position + (hailstone2.Direction * 1000000000000000),
            out var intersection);

        if (doesIntersect)
        {
            var inArea =
                intersection.X >= textAreaMin && intersection.X <= testAreaMax &&
                intersection.Y >= textAreaMin && intersection.Y <= testAreaMax;

            Console.WriteLine($"{hailstone1.Position} and {hailstone2.Position}: {intersection}, In area: {inArea}");

            if (inArea)
            {
                totalIntersections += 1;
            }
        }
        else
        {
            Console.WriteLine($"{hailstone1.Position} and {hailstone2.Position}: No intersection");
        }   
    }
}

Console.WriteLine($"Total intersections: {totalIntersections}");

// Taken from https://stackoverflow.com/questions/23896355/increment-string-value-from-aaa-to-aab-to-aac-and-so-on.
string GetNextPrefix(string prefix, string format = "000")
{
    const string Charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    //remove the ToUpper() if you want to expand the charset to contains both cases
    var values = prefix.ToUpper().Select(c => Charset.IndexOf(c));
    if (values.Any(v => v == -1))
        throw new Exception("Invalid Prefix : " + prefix);

    var value = values.Aggregate(0, (acc, v) => acc = acc * Charset.Length + v) + 1;
    var result = String.Empty;

    while (value != 0)
    {
        var remainder = value % Charset.Length;

        result = Charset[remainder] + result;
        value = (value - remainder) / Charset.Length;
    }

    return result.PadLeft(format.Length, Charset[0]);
}

// A bit of ChatGPT for collision detection.
bool TryFindIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
{
    intersection = new Vector2();

    var denominator = (p1.X - p2.X) * (p3.Y - p4.Y) - (p1.Y - p2.Y) * (p3.X - p4.X);
    if (denominator == 0)
    {
        return false; // Lines are parallel
    }

    var t = ((p1.X - p3.X) * (p3.Y - p4.Y) - (p1.Y - p3.Y) * (p3.X - p4.X)) / denominator;
    var u = -((p1.X - p2.X) * (p1.Y - p3.Y) - (p1.Y - p2.Y) * (p1.X - p3.X)) / denominator;

    // Check if intersection is within line segments
    if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
    {
        intersection.X = p1.X + t * (p2.X - p1.X);
        intersection.Y = p1.Y + t * (p2.Y - p1.Y);
        return true;
    }

    return false; // Intersection not within the line segments
}

record HailStone(string Id, Vector2 Position, Vector2 Direction);

record Coordinate
{
    public int X { get; set; }

    public int Y { get; set; }

    public int Z { get; set; }
}

record Movement(int X, int Y, int Z);