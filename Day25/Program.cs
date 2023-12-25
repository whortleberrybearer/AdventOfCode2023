var input = File.ReadAllLines("Input.txt");
var allNodes = new List<string>();

foreach (var line in input)
{
    Console.WriteLine(line);

    allNodes.AddRange(line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries));
}

var nodes = allNodes.Distinct().ToDictionary(n => n, n => new List<Link>());

foreach (var line in input)
{
    var parts = line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);

    foreach (var join in parts.Skip(1))
    {
        var wire = new Wire();

        nodes[parts[0]].Add(new Link(parts[0], join, wire));
        nodes[join].Add(new Link(join, parts[0], wire));
    }
}

Console.WriteLine();

record Link(string Start, string End, Wire Wire);

record Wire(bool Cut = false);