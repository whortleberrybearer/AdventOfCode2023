using System.Numerics;

var input = File.ReadAllLines("Input.txt");
var map = new int[input.Length][];
var paths = new List<Path>();
var visitedPaths = new List<Path>[input.Length][];

for (var lineIndex = 0; lineIndex < input.Length; lineIndex++)
{
    var line = input[lineIndex];

    Console.WriteLine($"Line: {line}");

    map[lineIndex] = line.Select(c => int.Parse(c.ToString())).ToArray();
    visitedPaths[lineIndex] = new List<Path>[map[lineIndex].Length];

    for (var i = 0; i < map[lineIndex].Length; i++)
    {
        visitedPaths[lineIndex][i] = new List<Path>();
    }
}

paths.Add(new Path(4, 0, new List<Vector2>())
{
    MoveX = 1,
    MoveY = 0,
    DirectionMoves = 4,
    TotalLoss = map[0][1] + map[0][2] + map[0][3] + map[0][4],
    Map = map,
});
visitedPaths[0][1].Add(paths[0]);

paths.Add(new Path(0, 4, new List<Vector2>())
{
    MoveX = 0,
    MoveY = 1,
    DirectionMoves = 4,
    TotalLoss = map[1][0] + map[2][0] + map[3][0] + map[4][0],
    Map = map,
});
visitedPaths[1][0].Add(paths[1]);

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
        var directionVisited = visitedPaths[newPath.Y][newPath.X].Where(p => p.MoveX == newPath.MoveX && p.MoveY == newPath.MoveY);

        if ((directionVisited.Count() > 0) && directionVisited.Any(p => (p.TotalLoss <= newPath.TotalLoss) && (p.DirectionMoves <= newPath.DirectionMoves)))
        {
            // We have previously been through here with a better score and fever moves, so no point continuing.
            continue;
        }
        else
        {
            visitedPaths[newPath.Y][newPath.X].Add(newPath);
            paths.Add(newPath);
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

        if ((DirectionMoves < 10) && CanMove(X + MoveX, Y + MoveY))
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
            if (CanMove(X, Y - 4))
            {
                nextPaths.Add(
                    new Path(X, Y - 4, Previous)
                    {
                        MoveX = 0,
                        MoveY = -1,
                        DirectionMoves = 4,
                        TotalLoss = TotalLoss + Map[Y - 1][X] + Map[Y - 2][X] + Map[Y - 3][X] + Map[Y - 4][X],
                        Map = Map,
                    });
            }

            if (CanMove(X, Y + 4))
            {
                nextPaths.Add(
                    new Path(X, Y + 4, Previous)
                    {
                        MoveX = 0,
                        MoveY = 1,
                        DirectionMoves = 4,
                        TotalLoss = TotalLoss + Map[Y + 1][X] + Map[Y + 2][X] + Map[Y + 3][X] + Map[Y + 4][X],
                        Map = Map,
                    });
            }
        }

        if (MoveY != 0)
        {
            if (CanMove(X - 4, Y))
            {
                nextPaths.Add(
                    new Path(X - 4, Y, Previous)
                    {
                        MoveX = -1,
                        MoveY = 0,
                        DirectionMoves = 4,
                        TotalLoss = TotalLoss + Map[Y][X - 1] + Map[Y][X - 2] + Map[Y][X - 3] + Map[Y][X - 4],
                        Map = Map,
                    });
            }

            if (CanMove(X + 4, Y))
            {
                nextPaths.Add(
                    new Path(X + 4, Y, Previous)
                    {
                        MoveX = 1,
                        MoveY = 0,
                        DirectionMoves = 4,
                        TotalLoss = TotalLoss + Map[Y][X + 1] + Map[Y][X + 2] + Map[Y][X + 3] + Map[Y][X + 4],
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