var input = File.ReadAllText("Input.txt");
string directions = null;
var nodes = new Dictionary<string, Node>();

foreach (var line in input.Split("\r\n"))
{
    Console.WriteLine($"Line: {line}");

    if (directions is null)
    {
        directions = line;
    }
    else if (!string.IsNullOrEmpty(line))
    {
        var parts = line.Split(new char[] { '=', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        nodes.Add(parts[0], new Node(parts[0], parts[1], parts[2]));
    }
}

var currentNode = nodes["AAA"];
var total = 0;

do
{
    for (var i = 0; i < directions.Length; i++)
    {
        ++total;

        if (directions[i] == 'L')
        {
            currentNode = nodes[currentNode.Left];
        }
        else
        {
            currentNode = nodes[currentNode.Right];
        }

        Console.WriteLine($"Current node: {currentNode.Key}");

        if (currentNode.Key == "ZZZ")
        {
            break;
        }
    }
}
while (currentNode.Key != "ZZZ");

Console.WriteLine($"Total: {total}");

record Node(string Key, string Left, string Right);