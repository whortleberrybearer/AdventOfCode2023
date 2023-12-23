var map = File.ReadAllLines("Input.txt").Select(l => l.ToCharArray()).ToArray();
Position startPosition = null;
Position endPosition = null;

for (var i = 0; i < map[0].Length; i++)
{
    if (map[0][i] == '.')
    {
        startPosition = new Position(i, 0);
    }

    if (map[map.Length - 1][i] == '.')
    {
        endPosition = new Position(i, map.Length - 1);
    }
}

// Remove the slopes.
var steps = new List<Route>[map.Length][];

for (var y = 0; y < map.Length; y++)
{
    steps[y] = new List<Route>[map[y].Length];

    for (var x = 0; x < map[y].Length; x++)
    {
        steps[y][x] = new List<Route>();

        if ((map[y][x] == '<') || (map[y][x] == '>') || (map[y][x] == 'v'))
        {
            map[y][x] = '.';
        }
    }
}

var endRoutes = new List<Route>();
var routes = new List<Route>()
{
    new Route(startPosition, 0, 0, Enumerable.Empty<Position>()),
};

/* Part 1
do
{
    var route = routes.First();
    routes.Remove(route);

    Console.WriteLine($"Checking route: {route.Position}");

    if ((route.Position.X == endPosition.X) && (route.Position.Y == endPosition.Y))
    {
        endRoutes.Add(route);

        Console.WriteLine($"Reached end in {route.PreviousMoves.Count()} steps");
    }
    else
    {
        routes.AddRange(route.CheckRoutes(map));
    }
}
while (routes.Any());
*/

var junctions = new List<Junction>();

do
{
    var route = routes.First();
    routes.Remove(route);

    Console.WriteLine($"Checking route: {route.Position}");

    var newRoutes = route.CheckRoutes(map);

    if ((newRoutes.Count() > 1) || ((route.Position.X == endPosition.X) && (route.Position.Y == endPosition.Y)))
    {
        // We have found a junction.
        var junction = new Junction(route.PreviousMoves.Append(route.Position).ToArray());

        // If we already have a junction ending here, we will have expanded from it already.
        if (!junctions.Any(j => j.End.X == junction.End.X && j.End.Y == junction.End.Y))
        {
            // Not been here before, so start a new expansion.
            foreach (var newRoute in newRoutes)
            {
                routes.Add(new Route(newRoute.Position, newRoute.MoveX, newRoute.MoveY, new Position[] { route.Position }));
            }
        }

        // Can get too and from, so add the inverse route too.
        junctions.Add(junction);
        junctions.Add(new Junction(junction.PreviousMoves.Reverse().ToArray()));
    }
    else
    {
        // Just continue along the path.
        routes.AddRange(newRoutes);
    }
}
while (routes.Any());

var junctionPositions = junctions.Select(j => j.Start).Distinct();
var nodes = new Dictionary<Position, List<Junction>>();

foreach (var junctionPosition in junctionPositions)
{
    nodes.Add(junctionPosition, new List<Junction>());
}

foreach (var junction in junctions)
{
    if (!nodes[junction.Start].Any(j => j.End == junction.End))
    {
        nodes[junction.Start].Add(junction);
    }
}

var nodeRoutes = new List<NodeRoute>()
{
    new NodeRoute(startPosition, Enumerable.Empty<Position>()),
};
var endNodeRoutes = new List<NodeRoute>();
var longestNodeRoute = new NodeRoute(startPosition, Enumerable.Empty<Position>());

do
{
    var nodeRoute = nodeRoutes.First();
    nodeRoutes.Remove(nodeRoute);

    Console.WriteLine($"Checking node route: {nodeRoute.Position}");

    if (nodeRoute.Position == endPosition)
    {
        // endNodeRoutes.Add(nodeRoute);

        Console.WriteLine($"Reached end in {nodeRoute.PreviousPositions.Count()} steps");

        if (nodeRoute.PreviousPositions.Count() > longestNodeRoute.PreviousPositions.Count())
        {
            longestNodeRoute = nodeRoute;
        }
    }
    else
    {
        foreach (var link in nodes[nodeRoute.Position])
        {
            if (!nodeRoute.PreviousPositions.Contains(link.End))
            {
                nodeRoutes.Add(new NodeRoute(link.End, nodeRoute.PreviousPositions.Concat(link.PreviousMoves.Skip(1))));
            }
        }
    }
}
while (nodeRoutes.Any());


Console.WriteLine($"Longest hike: {longestNodeRoute.PreviousPositions.Count()}");

record Route(Position Position, int MoveX, int MoveY, IEnumerable<Position> PreviousMoves)
{
    public IEnumerable<Route> CheckRoutes(char[][] map)
    {
        var routes = new List<Route>();
        var move = map[Position.Y][Position.X];

        if (move == '.' || move == '>')
        {
            var route = CheckNewRoute(Position.X + 1, Position.Y, 1, 0, map);

            if (route != null)
            {
                routes.Add(route);
            }
        }

        if (move == '.' || move == '<')
        {
            var route = CheckNewRoute(Position.X - 1, Position.Y, -1, 0, map);

            if (route != null)
            {
                routes.Add(route);
            }
        }

        if (move == '.' || move == 'v')
        {
            var route = CheckNewRoute(Position.X, Position.Y + 1, 0, 1, map);

            if (route != null)
            {
                routes.Add(route);
            }
        }

        if (move == '.')
        {
            var route = CheckNewRoute(Position.X, Position.Y - 1, 0, -1, map);

            if (route != null)
            {
                routes.Add(route);
            }
        }

        return routes;
    }

    Route? CheckNewRoute(int x, int y, int moveX, int moveY, char[][] map)
    {
        if (!CanMove(x, y, map))
        {
            return null;
        }

        return new Route(new Position(x, y), moveX, moveY, PreviousMoves.Append(Position));
    }

    bool CanMove(int x, int y, char[][] map)
    {
        if (PreviousMoves.Any(p => p.X == x && p.Y == y))
        {
            return false;
        }

        if (x < 0 || y < 0)
        {
            return false;
        }

        if ((x >= map[0].Length) || (y >= map.Length))
        {
            return false;
        }

        if (map[y][x] == '#')
        {
            return false;
        }

        return true;
    }
}

record Position(int X, int Y);

record Junction(IEnumerable<Position> PreviousMoves)
{
    public Position Start => PreviousMoves.First();
    
    public Position End => PreviousMoves.Last();

    public int Steps = PreviousMoves.Count();
}

record NodeRoute(Position Position, IEnumerable<Position> PreviousPositions)
{
    public int Steps => PreviousPositions.Count();
}