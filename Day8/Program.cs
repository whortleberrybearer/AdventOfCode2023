using Arithmetic;

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

var totalMoves = new List<int>();

foreach (var node in nodes.Where(n => n.Key.EndsWith("A")))
{
    var moves = 0;
    var currentNode = node.Value;
    var previousHits = new List<string>();

    do
    {
        for (var i = 0; i < directions.Length; i++)
        {
            ++moves;

            if (directions[i] == 'L')
            {
                currentNode = nodes[currentNode.Left];
            }
            else
            {
                currentNode = nodes[currentNode.Right];
            }

            Console.WriteLine($"Current node: {currentNode.Key}, Moves: {moves}");

            if (currentNode.Key.EndsWith("Z"))
            {
                break;
            }
        }
    }
    while (!currentNode.Key.EndsWith("Z"));

    totalMoves.Add(moves);

    Console.WriteLine();
}

LCM lcm = new LCM(totalMoves);

var total = lcm.getLCM();

Console.WriteLine($"Total: {total}");

record Node(string Key, string Left, string Right);