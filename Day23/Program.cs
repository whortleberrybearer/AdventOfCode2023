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

var endRoutes = new List<Route>();
var routes = new List<Route>()
{
    new Route(startPosition,Enumerable.Empty<Position>()),
};

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

Console.WriteLine($"Longest hike: {endRoutes.Max(r => r.PreviousMoves.Count())}");

record Route(Position Position, IEnumerable<Position> PreviousMoves)
{
    public IEnumerable<Route> CheckRoutes(char[][] map)
    {
        var routes = new List<Route>();
        var move = map[Position.Y][Position.X];

        if (move == '.' || move == '>')
        {
            var route = CheckNewRoute(Position.X + 1, Position.Y, map);

            if (route != null)
            {
                routes.Add(route);
            }
        }

        if (move == '.' || move == '<')
        {
            var route = CheckNewRoute(Position.X - 1, Position.Y, map);

            if (route != null)
            {
                routes.Add(route);
            }
        }

        if (move == '.' || move == 'v')
        {
            var route = CheckNewRoute(Position.X, Position.Y + 1, map);

            if (route != null)
            {
                routes.Add(route);
            }
        }

        if (move == '.')
        {
            var route = CheckNewRoute(Position.X, Position.Y - 1, map);

            if (route != null)
            {
                routes.Add(route);
            }
        }

        return routes;
    }

    Route? CheckNewRoute(int x, int y, char[][] map)
    {
        if (!CanMove(x, y, map))
        {
            return null;
        }

        return new Route(new Position(x, y), PreviousMoves.Append(Position));
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
