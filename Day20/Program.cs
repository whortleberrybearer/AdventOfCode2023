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
var pulsesSent = new List<PulsesSent>();
var endStates = new List<string>();
var repeatIndex = -1;

do
{
    var sent = broadcaster.PushButton();
    var states = string.Concat(modules.Values.Select(m => m.State));

    repeatIndex = endStates.IndexOf(states);

    if (repeatIndex == -1)
    {
        endStates.Add(states);   
        pulsesSent.Add(sent);
    }
} 
while (repeatIndex == -1 || pulsesSent.Count == 1000);

var highSent = pulsesSent.Take(repeatIndex).Sum(s => s.High);
var lowSent = pulsesSent.Take(repeatIndex).Sum(s => s.Low);

var repeat = (pulsesSent.Count - repeatIndex);
var multiplier = 1000 / repeat;
var remainder = 1000 % repeat;

highSent += pulsesSent.Skip(repeatIndex).Sum(s => s.High) * multiplier;
lowSent += pulsesSent.Skip(repeatIndex).Sum(s => s.Low) * multiplier;

highSent += pulsesSent.Skip(repeatIndex).Take(remainder).Sum(s => s.High);
lowSent += pulsesSent.Skip(repeatIndex).Take(remainder).Sum(s => s.Low);

var totalSent = highSent * lowSent;

Console.WriteLine($"Pulses sent: {totalSent}");
/*
broadcaster.PushButton();

Console.WriteLine();

broadcaster.PushButton();

Console.WriteLine();

broadcaster.PushButton();

var sss = 0;
*/
record Module(string Id)
{
    public List<Module> Links { get; } = new List<Module>();

    public virtual string State => string.Empty;

    public virtual IEnumerable<NextPulse> SendPulse(bool high, string source)
    {
        // Untyped module.  Do nothing.
        return Enumerable.Empty<NextPulse>();
    }
}

record BroadcastModule(string Id) : Module(Id)
{
    public PulsesSent PushButton()
    {
        var pulsesSent = new PulsesSent();
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
                if (pulseToSend.High)
                {
                    Console.WriteLine($"{pulseToSend.Source} -high-> {pulseToSend.Module.Id}");

                    pulsesSent.High += 1;
                }
                else
                {
                    Console.WriteLine($"{pulseToSend.Source} -low-> {pulseToSend.Module.Id}");
                    
                    pulsesSent.Low += 1;
                }

                nextPulses.AddRange(pulseToSend.Module.SendPulse(pulseToSend.High, pulseToSend.Source));
            }
        } 
        while (nextPulses.Any());

        return pulsesSent;
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
    
    public override string State => On ? "1" : "0";

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

    public override string State => string.Concat(ConnectedStates.Values.Select(v => v ? "1" : "0"));
    
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

record PulsesSent
{
    public int High { get; set; } = 0;
    public int Low { get; set; } = 0;
};