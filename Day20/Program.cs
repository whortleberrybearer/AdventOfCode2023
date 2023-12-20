var input = File.ReadAllLines("Input.txt");
var modules = new Dictionary<string, Module>();

Console.WriteLine("Populating modules");

foreach (var line in input)
{
    Console.WriteLine($"Line: {line}");

    var parts = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

    if (parts[0] == "broadcaster")
    {
        modules.Add(parts[0], new BroadcastModule(parts[0]));
    }
    else if (parts[0][0] == '%')
    {
        var id = parts[0][1..];
        
        modules.Add(id, new ConjunctionModule(id));
    }
    else if (parts[0][0] == '&')
    {
        var id = parts[0][1..];
        
        modules.Add(id, new FlipFlopModule(id));
    }
}

Console.WriteLine();
Console.WriteLine("Linking modules");

foreach (var line in input)
{
    Console.WriteLine($"Line: {line}");

    var parts = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
    
    var module = modules[parts[0].TrimStart('%', '&')];

    foreach (var part in parts.Skip(2))
    {
        if (modules.TryGetValue(part, out var linkModule))
        {
            module.Links.Add(linkModule);
        }
    }
}

Console.WriteLine();

var sss = 0;

record Module(string Id)
{
    public List<Module> Links { get; } = new List<Module>();
}

record BroadcastModule(string Id) : Module(Id)
{
    
}

record FlipFlopModule(string Id) : Module(Id)
{
    
}

record ConjunctionModule(string Id) : Module(Id)
{
    
}