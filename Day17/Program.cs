using System.Numerics;

var input = File.ReadAllLines("Input.txt");
var map = new int[input.Length][];
var paths = new List<Path>();
var minimumHeatLoss = int.MaxValue;
var shortestPaths = new Path[input.Length][];

for (var lineIndex = 0; lineIndex < input.Length; lineIndex++)
{
    var line = input[lineIndex];

    Console.WriteLine($"Line: {line}");

    map[lineIndex] = line.Select(c => int.Parse(c.ToString())).ToArray();
    shortestPaths[lineIndex] = new Path[map[lineIndex].Length];
}

paths.Add(new Path(1, 0, new List<Vector2>())
{
    MoveX = 1,
    MoveY = 0,
    DirectionMoves = 1,
    TotalLoss = map[0][1],
    Map = map,
});
shortestPaths[0][1] = paths[0];

paths.Add(new Path(0, 1, new List<Vector2>())
{
    MoveX = 0,
    MoveY = 1,
    DirectionMoves = 1,
    TotalLoss = map[1][0],
    Map = map,
});
shortestPaths[1][0] = paths[1];

Path? shortestPath = null;

do
{
    // Ensure lowest total cost is at the top of the list.
    paths.Sort((x, y) => x.TotalLoss - y.TotalLoss);

    // Expand from the lowest position.
    var path = paths.First();
    paths.Remove(path);

    Console.WriteLine($"Y: {path.Y}, X: {path.X}, Total: {path.TotalLoss}");

    var newPaths = path.Move().Where(p => p.TotalLoss < (shortestPath?.TotalLoss ?? int.MaxValue));

    foreach (var newPath in newPaths)
    {
        var curentShortest = shortestPaths[newPath.Y][newPath.X];

        if ((curentShortest != null) && (newPath.TotalLoss > (curentShortest.TotalLoss + 20)))
        {
            continue;
        }

        if ((curentShortest == null) ||
            ((newPath.TotalLoss < curentShortest.TotalLoss) && (newPath.DirectionMoves <= curentShortest.DirectionMoves)))
        {
            // This is the best way of getting here, so remove any other paths.
            paths.RemoveAll(p => p.X == newPath.X && p.Y == newPath.Y);

            shortestPaths[newPath.Y][newPath.X] = newPath;

            paths.Add(newPath);
        }
        else if ((newPath.TotalLoss <= curentShortest.TotalLoss) || (newPath.DirectionMoves < curentShortest.DirectionMoves))
        {
            if (!paths.Any(p => p.X == newPath.X && p.Y == newPath.Y && p.TotalLoss <= newPath.TotalLoss && p.DirectionMoves < newPath.DirectionMoves))
            {
                paths.Add(newPath);
                paths.RemoveAll(p => p.X == newPath.X && p.Y == newPath.Y && p.TotalLoss > newPath.TotalLoss && p.DirectionMoves >= newPath.DirectionMoves);
            }
        }
    }

    var endPath = newPaths.FirstOrDefault(p => (p.Y == (input.Length - 1)) && (p.X == (map[0].Length - 1)));

    if (endPath != null)
    {
        shortestPath = endPath;
    }
}
while (paths.Any());

Console.WriteLine($"Minimum heat loss: {shortestPath!.TotalLoss}");
Console.WriteLine(string.Join(" -> ", shortestPath.Previous));

class Path
{
    public Path(int x, int y, List<Vector2> previous)
    {
        X = x;
        Y = y;
        Previous = new List<Vector2>(previous);

        Previous.Add(new Vector2(X, Y));
    }

    public int X { get; }

    public int Y { get; }

    public int MoveX { get; set; }

    public int MoveY { get; set; }

    public int DirectionMoves { get; set; }

    public int TotalLoss { get; set; }

    public int[][] Map { get; set; }

    public List<Vector2> Previous { get; }

    public IEnumerable<Path> Move()
    {
        var nextPaths = new List<Path>();

        if ((DirectionMoves < 3) && CanMove(X + MoveX, Y + MoveY))
        {
            nextPaths.Add(
                new Path(X + MoveX, Y + MoveY, Previous)
                {
                    MoveX = MoveX,
                    MoveY = MoveY,
                    DirectionMoves = DirectionMoves + 1,
                    TotalLoss = TotalLoss + Map[Y + MoveY][X + MoveX],
                    Map = Map,
                });
        }

        if (MoveX != 0)
        {
            if (CanMove(X, Y - 1))
            {
                nextPaths.Add(
                    new Path(X, Y - 1, Previous)
                    {
                        MoveX = 0,
                        MoveY = -1,
                        DirectionMoves = 1,
                        TotalLoss = TotalLoss + Map[Y - 1][X],
                        Map = Map,
                    });
            }

            if (CanMove(X, Y + 1))
            {
                nextPaths.Add(
                    new Path(X, Y + 1, Previous)
                    {
                        MoveX = 0,
                        MoveY = 1,
                        DirectionMoves = 1,
                        TotalLoss = TotalLoss + Map[Y + 1][X],
                        Map = Map,
                    });
            }
        }

        if (MoveY != 0)
        {
            if (CanMove(X - 1, Y))
            {
                nextPaths.Add(
                    new Path(X - 1, Y, Previous)
                    {
                        MoveX = -1,
                        MoveY = 0,
                        DirectionMoves = 1,
                        TotalLoss = TotalLoss + Map[Y][X - 1],
                        Map = Map,
                    });
            }

            if (CanMove(X + 1, Y))
            {
                nextPaths.Add(
                    new Path(X + 1, Y, Previous)
                    {
                        MoveX = 1,
                        MoveY = 0,
                        DirectionMoves = 1,
                        TotalLoss = TotalLoss + Map[Y][X + 1],
                        Map = Map,
                    });
            }
        }

        return nextPaths;
    }

    private bool CanMove(int X, int Y)
    {
        if (X < 0 || Y < 0)
        {
            return false;
        }

        if ((X >= Map[0].Length) || (Y >= Map.Length))
        {
            return false;
        }

        return !Previous.Contains(new Vector2(X, Y));
    }
}