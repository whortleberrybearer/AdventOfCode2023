var input = File.ReadAllLines("Input.txt");
var lineIndex = 0;
var line = input[lineIndex++];
var parts = new List<Part>();
var workflows = new Dictionary<string, Workflow>();

while (!string.IsNullOrEmpty(line))
{
    Console.WriteLine(line);

    var split = line.Split(new char[] { '{', '}', ',' }, StringSplitOptions.RemoveEmptyEntries);

    var conditions = new List<WorkflowCondition>();

    for (var i = 1; i < split.Length - 1; i++)
    {
        var components = split[i].Split(':');
        
        conditions.Add(new WorkflowCondition(
            components[0][0].ToString(),
            components[0][1].ToString(),
            int.Parse(components[0].Substring(2)),
            components[1]));
    }
    
    workflows.Add(split.First(), new Workflow(split.First(), conditions, split.Last()));
    
    line = input[lineIndex++];
}

line = input[lineIndex++];

while (!string.IsNullOrEmpty(line))
{
    Console.WriteLine(line);

    var specification = new Dictionary<string, int>();

    foreach (var spec in line.Trim('{', '}').Split(','))
    {
        var split = spec.Split('=');
        
        specification.Add(split[0], int.Parse(split[1]));
    }
    
    parts.Add(new Part(specification));

    if (lineIndex == input.Length)
    {
        break;
    }

    line = input[lineIndex++];
}

Console.WriteLine();

var accepted = new List<Part>();
/*
foreach (var part in parts)
{
    // Start the workflow on the "In" key.
    var result = "in";
    
    do
    {
        result = workflows[result].Process(part);    
    } 
    while (result != "A" && result != "R");

    if (result == "A")
    {
        Console.WriteLine($"Accepted part: {part}");

        accepted.Add(part);
    }
}
*/
Console.WriteLine($"Total value: {accepted.Sum(p => p.TotalValue)}");


var ranges = Something(
    workflows["in"],
    new PartRange(new Dictionary<string, Range>()
    {
        { "x", new Range() },
        { "m", new Range() },
        { "a", new Range() },
        { "s", new Range() }
    }));

var dsfas = 0;

IEnumerable<PartRange> Something(Workflow workflow, PartRange partRange)
{
    var ranges = new List<PartRange>();

    foreach (var condition in workflow.Conditions)
    {
        if (condition.Action != "R")
        {
            var newRange = CopyRange(partRange);

            if (condition.Operation == ">")
            {
                newRange.Values[condition.Character].Min =
                    Math.Max(newRange.Values[condition.Character].Min, condition.Value + 1);
            }
            else if (condition.Operation == "<")
            {
                newRange.Values[condition.Character].Max =
                    Math.Min(newRange.Values[condition.Character].Max, condition.Value - 1);
            }

            if (condition.Action == "A")
            {
                ranges.Add(newRange);
            }
            else
            {
                ranges.AddRange(Something(workflows[condition.Action], newRange));
            }
        }
    }

    // The default action is the inverse of all the conditions.
    if (workflow.Default != "R")
    {
        var newRange = CopyRange(partRange);
        
        foreach (var condition in workflow.Conditions)
        {
            if (condition.Operation == ">")
            {
                newRange.Values[condition.Character].Max =
                    Math.Min(newRange.Values[condition.Character].Max, condition.Value);
            }
            else if (condition.Operation == "<")
            {
                newRange.Values[condition.Character].Min =
                    Math.Max(newRange.Values[condition.Character].Min, condition.Value);
            }
        }

        if (workflow.Default == "A")
        {
            ranges.Add(newRange);
        }
        else
        {
            ranges.AddRange(Something(workflows[workflow.Default], newRange));
        }
    }

    return ranges;
}

PartRange CopyRange(PartRange range)
{
    return new PartRange(new Dictionary<string, Range>()
    {
        { "x", new Range() { Min = range.Values["x"].Min, Max = range.Values["x"].Max }},
        { "m", new Range() { Min = range.Values["m"].Min, Max = range.Values["m"].Max }},
        { "a", new Range() { Min = range.Values["a"].Min, Max = range.Values["a"].Max }},
        { "s", new Range() { Min = range.Values["s"].Min, Max = range.Values["s"].Max }},
    });
}

record Workflow(string Id, IEnumerable<WorkflowCondition> Conditions, string Default)
{
    public string Process(Part part)
    {
        foreach (var condition in Conditions)
        {
            var partValue = part.Values[condition.Character];

            if (condition.Operation == "<")
            {
                if (partValue < condition.Value)
                {
                    return condition.Action;
                }
            }
            else if (condition.Operation == ">")
            {
                if (partValue > condition.Value)
                {
                    return condition.Action;
                }
            }
        }
        
        return Default;
    }
}

record WorkflowCondition(string Character, string Operation, int Value, string Action);

record Part(Dictionary<string, int> Values)
{
    public int TotalValue => Values.Values.Sum(v => v);
}

record Range
{
    public int Min { get; set; } = 1;

    public int Max { get; set; } = 4000;
};

record PartRange(Dictionary<string, Range> Values);