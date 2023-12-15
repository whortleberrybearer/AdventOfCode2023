var input = File.ReadLines("Input.txt");
var hashTotal = 0;
var boxes = new List<Lens>[256];

for (var i = 0; i < boxes.Count(); i++)
{
    boxes[i] = new List<Lens>();
}

foreach (var line in input)
{
    Console.WriteLine(line);

    foreach (var part in line.Split(','))
    {
        var sections = part.Split(new char[] { '-', '=' }, StringSplitOptions.RemoveEmptyEntries);

        var hash = CalculateHash(sections[0]);

        Console.WriteLine($"Part: {part}, Hash: {hash}");

        var box = boxes[hash];

        if (sections.Length == 1)
        {
            // Must be a '-'.
            var lens = box.Find(l => l.Label == sections[0]);

            if (lens is not null)
            {
                box.Remove(lens);
            }
        }
        else
        {
            var lens = box.Find(l => l.Label == sections[0]);
            
            if (lens is not null)
            {
                lens.FocalLength = int.Parse(sections[1]);
            }
            else
            {
                lens = new Lens(sections[0])
                {
                    FocalLength = int.Parse(sections[1]), 
                };
                
                box.Add(lens);
            }
        }
        
        hashTotal += hash;
    }
}

Console.WriteLine($"Hash total: {hashTotal}");

var focusingPower = 0;

for (var i = 0; i < boxes.Length; i++)
{
    var box = boxes[i];

    for (var j = 0; j < box.Count; j++)
    {
        focusingPower += (i + 1) * (j + 1) * box[j].FocalLength;
    }
}

Console.WriteLine($"Focusing power: {focusingPower}");

int CalculateHash(string s)
{
    var hash = 0;

    foreach (var c in s.ToCharArray())
    {
        var ascii = (int)c;

        hash += ascii;
        hash *= 17;
        hash %= 256;
    }

    return hash;
}

record Lens(string Label)
{
    public int FocalLength { get; set; }
};