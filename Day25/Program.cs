var input = File.ReadAllLines("Input.txt");
var allNodes = new List<string>();

foreach (var line in input)
{
    Console.WriteLine(line);

    allNodes.AddRange(line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries));
}

var nodes = allNodes.Distinct().ToDictionary(n => n, n => new List<Wire>());

foreach (var line in input)
{
    var parts = line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);

    foreach (var join in parts.Skip(1))
    {
        var wire = new Wire(parts[0], join);

        nodes[parts[0]].Add(wire);
        nodes[join].Add(wire);
    }
}

Console.WriteLine();
var routes = new Dictionary<string, IEnumerable<Wire>>();

for (var i = 0; i < nodes.Count; i++)
{
    for (var j = i + 1; j < nodes.Count; j++)
    {
        var path = FindRoute(nodes.Keys.ElementAt(i), nodes.Keys.ElementAt(j));

        if (path != null)
        {
            Console.WriteLine($"{nodes.Keys.ElementAt(i)}->{nodes.Keys.ElementAt(j)} = {string.Join(" - ", path.Select(p => $"{p.Start}-{p.End}"))}");

            routes.Add($"{nodes.Keys.ElementAt(i)}-{nodes.Keys.ElementAt(j)}", path.ToArray());
            routes.Add($"{nodes.Keys.ElementAt(j)}-{nodes.Keys.ElementAt(i)}", path.ToArray());
        }
        else
        {
            // No path for some reason.
            int xxx = 0;
        }
    }
}

Console.WriteLine();

var groups = routes.Values.SelectMany(r => r).GroupBy(w => w).OrderByDescending(g => g.Count());

foreach(var group in groups)
{
    Console.WriteLine($"{group.Key} = {group.Count()}");
}

Console.WriteLine();

IEnumerable<Wire>? FindRoute(string start, string end)
{
    var routes = new List<Route>()
    {
        new Route(start, null, Enumerable.Empty<Wire>()),
    };

    do
    {
        var route = routes.First();
        routes.Remove(route);

        if (route.Node == "pzl")
        {
            int i = 0;
        }

        var previousWires = route.PreviousWires;

        if (route.Wire != null)
        {
            previousWires = route.PreviousWires.Append(route.Wire).ToArray();
        }

        if (route.Node == end)
        {
            return previousWires;
        }
        else
        {
            foreach (var link in nodes[route.Node])
            {
                if (!previousWires.Contains(link))
                {
                    routes.Add(new Route(route.Node == link.Start ? link.End : link.Start, link, previousWires));
                }
            }
        }
    }
    while (routes.Count > 0);

    return null;
}

record Route(string Node, Wire Wire, IEnumerable<Wire> PreviousWires);

record Wire(string Start, string End, bool Cut = false);