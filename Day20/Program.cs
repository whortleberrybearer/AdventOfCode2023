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
    else if (parts[0][0] == '&')
    {
        var id = parts[0][1..];
        
        modules.Add(id, new ConjunctionModule(id));
    }
    else if (parts[0][0] == '%')
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
        if (!modules.TryGetValue(part, out var linkModule))
        {
            // Some modules are undefined, so need to add them.
            linkModule = new Module(part);
            
            modules.Add(linkModule.Id, linkModule);
        }

        module.Links.Add(linkModule);

        // Need to store the state in the conjunction module as well.
        if (linkModule is ConjunctionModule conjunctionModule)
        {
            conjunctionModule.ConnectedStates.Add(module.Id, false);
        }
    }
}

Console.WriteLine();

var broadcaster = modules["broadcaster"] as BroadcastModule;

broadcaster.PushButton();

Console.WriteLine();

broadcaster.PushButton();

Console.WriteLine();

broadcaster.PushButton();

Console.WriteLine();

broadcaster.PushButton();

var sss = 0;

record Module(string Id)
{
    public List<Module> Links { get; } = new List<Module>();

    public virtual IEnumerable<NextPulse> SendPulse(bool high, string source)
    {
        // Untyped module.  Do nothing.
        return Enumerable.Empty<NextPulse>();
    }
}

record BroadcastModule(string Id) : Module(Id)
{
    public void PushButton()
    {
        var nextPulses = new List<NextPulse>()
        {
            new NextPulse(this, false, "button"),
        };

        do
        {
            var pulsesToSend = nextPulses.ToArray();
            nextPulses.Clear();
            
            foreach (var pulseToSend in pulsesToSend)
            {
                var pulse = pulseToSend.High ? "high" : "low";
                
                Console.WriteLine($"{pulseToSend.Source} -{pulse}-> {pulseToSend.Module.Id}");
                
                nextPulses.AddRange(pulseToSend.Module.SendPulse(pulseToSend.High, pulseToSend.Source));
            }
        } 
        while (nextPulses.Any());
    }

    public override IEnumerable<NextPulse> SendPulse(bool high, string source)
    {
        var nextPulses = new List<NextPulse>();

        foreach (var link in Links)
        {
            nextPulses.Add(new NextPulse(link, high, Id));
        }

        return nextPulses;
    }
}

record FlipFlopModule(string Id) : Module(Id)
{
    public bool On { get; private set; } = false;

    public override IEnumerable<NextPulse> SendPulse(bool high, string source)
    {
        var nextPulses = new List<NextPulse>();

        // Do nothing for a high pulse.
        if (!high)
        {
            On = !On;
            
            foreach (var link in Links)
            {
                nextPulses.Add(new NextPulse(link, On, Id));
            }
        }

        return nextPulses;
    }
}

record ConjunctionModule(string Id) : Module(Id)
{
    public Dictionary<string, bool> ConnectedStates { get; set; } = new Dictionary<string, bool>();

    public override IEnumerable<NextPulse> SendPulse(bool high, string source)
    {
        var nextPulses = new List<NextPulse>();

        ConnectedStates[source] = high;

        foreach (var link in Links)
        {
            // Send high pulse if not all high.
            var nextPulse = ConnectedStates.Values.Any(s => s != true);
            
            nextPulses.Add(new NextPulse(link, nextPulse, Id));
        }

        return nextPulses;
    }
}

record NextPulse(Module Module, bool High, string Source);