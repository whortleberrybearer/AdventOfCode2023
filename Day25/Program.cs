using System.Text.Json.Serialization;

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

        if ((parts[0] == "kfr" && join == "vkp")
            || (parts[0] == "qpp" && join == "vnm") 
            || (parts[0] == "rhk" && join == "bff")
            )
        {
            wire.Cut = true;
        }
        

        nodes[parts[0]].Add(wire);
        nodes[join].Add(wire);
    }
}

Console.WriteLine();

var wiresCut = new List<Wire>();

for (var i = 0; i < 3; i++)
{
    var routes = new Dictionary<string, (IEnumerable<string> Nodes, IEnumerable<Wire> Wires)>();
    
    routes = CalcaulareNodeRoutes(routes);

    var groups = routes.Values.SelectMany(r => r.Wires).GroupBy(w => w).OrderBy(g => g.Count()).ToArray();

    foreach (var group in groups)
    {
        Console.WriteLine($"{group.Key} = {group.Count()}");
    }

    var wire = groups.Last().Key;

    wire.Cut = true;

    Console.WriteLine($"Cut wire {wire}");

    wiresCut.Add(wire);

    // Remove any routes that need re-expanding.
    foreach (var route in routes.ToArray())
    {
        if (route.Value.Wires.Contains(wire))
        {
            routes.Remove(route.Key);

            Console.WriteLine($"Need to re-expand {route.Key}");
        }
    }    

    Console.WriteLine();
}


//wiresCut.Add(new Wire("kfr", "vkp"));

CalculateSections(wiresCut);

void CalculateSections(IEnumerable<Wire> wiresCut)
{
    var wire = wiresCut.First();
    var nodesToCheck = new List<string>()
    {
        wire.Start
    };
    var visited = new List<string>()
    {
        wire.Start
    };

    do
    {
        var node = nodesToCheck.First();
        nodesToCheck.Remove(node);

        foreach (var link in nodes[node].Where(w => !w.Cut))
        {
            var linkNode = node == link.Start ? link.End : link.Start;

            if (!visited.Contains(linkNode))
            {
                visited.Add(linkNode);

                Console.WriteLine($"Connected: {linkNode}");

                nodesToCheck.Add(linkNode);
            }
        }
    }
    while (nodesToCheck.Any());
    
    var disconnectedCount = nodes.Count - visited.LongCount();

    var total = disconnectedCount * visited.LongCount();

    Console.WriteLine($"Connected: {visited.LongCount()},  Disconnected: {disconnectedCount}, Total: {total}");
}

Dictionary<string, (IEnumerable<string> Nodes, IEnumerable<Wire> Wires)> CalcaulareNodeRoutes(Dictionary<string, (IEnumerable<string> Nodes, IEnumerable<Wire> Wires)> routes)
{
    for (var i = 0; i < nodes.Count; i++)
    {
        for (var j = i + 1; j < nodes.Count; j++)
        {
            if (routes.ContainsKey($"{nodes.Keys.ElementAt(i)}-{nodes.Keys.ElementAt(j)}"))
            {
                continue;
            }

            var route = FindRoute(nodes.Keys.ElementAt(i), nodes.Keys.ElementAt(j), routes);

            if (route != null)
            {
                Console.WriteLine($"{nodes.Keys.ElementAt(i)}->{nodes.Keys.ElementAt(j)} = {string.Join(" - ", route.Value.Wires.Select(p => $"{p.Start}-{p.End}"))}");

                for (var i1 = 0; i1 < route.Value.Nodes.Count(); i1++)
                {
                    for (var j1 = i1 + 1; j1 < route.Value.Nodes.Count(); j1++)
                    {
                        var start = route.Value.Nodes.ElementAt(i1);
                        var end = route.Value.Nodes.ElementAt(j1);

                        if (!routes.ContainsKey($"{start}-{end}") &&
                            !routes.ContainsKey($"{end}-{start}"))
                        {
                            var takeCount = (j1 - i1) + 1;
                            var routeNodes = route.Value.Nodes.Skip(i1).Take(takeCount);
                            var routeWires = route.Value.Wires.Skip(i1).Take(takeCount - 1);

                            routes.Add($"{start}-{end}", (routeNodes.ToArray(), routeWires.ToArray()));
                            routes.Add($"{end}-{start}", (routeNodes.Reverse().ToArray(), routeWires.Reverse().ToArray()));
                        }
                    }
                }
            }
            else
            {
                // No path for some reason.
                int xxx = 0;
            }
        }
    }

    Console.WriteLine();

    return routes;
}

(IEnumerable<string> Nodes, IEnumerable<Wire> Wires)? FindRoute(string start, string end, Dictionary<string, (IEnumerable<string> Nodes, IEnumerable<Wire> Wires)> routes)
{
    var expandingRoutes = new List<Route>()
    {
        new Route(start, Enumerable.Empty<string>(), null, Enumerable.Empty<Wire>()),
    };

    do
    {
        var route = expandingRoutes.First();
        expandingRoutes.Remove(route);

        var previousWires = route.PreviousWires;
        var previousNodes = route.PreviousNodes;

        if (route.Wire != null)
        {
            previousWires = previousWires.Append(route.Wire).ToArray();
        }

        if (route.Node != null)
        {
            previousNodes = previousNodes.Append(route.Node).ToArray();
        }

        if (route.Node == end)
        {
            return (previousNodes, previousWires);
        }

        if (routes.TryGetValue($"{route.Node}-{end}", out var path))
        {
            return (previousNodes.Concat(path.Nodes.Skip(1)), previousWires.Concat(path.Wires));
        }

        foreach (var link in nodes[route.Node].Where(n => n.Cut == false))
        {
            if (!previousWires.Contains(link))
            {
                var routeNode = route.Node == link.Start ? link.End : link.Start;

                if (!expandingRoutes.Any(r => r.Node == routeNode))
                {
                    expandingRoutes.Add(new Route(routeNode, previousNodes, link, previousWires));
                }
            }
        }
    }
    while (expandingRoutes.Count > 0);

    return null;
}

record Route(string Node, IEnumerable<string> PreviousNodes, Wire Wire, IEnumerable<Wire> PreviousWires);

record Wire(string Start, string End)
{
    public bool Cut { get; set; }
}