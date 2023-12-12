var input = File.ReadLines("Input.txt");
var rows = new List<Row>();

foreach (var line in input)
{
    Console.WriteLine(line);
    
    var parts = line.Split(' ');
    
    rows.Add(new Row(parts[0], parts[1].Split(',').Select(c => int.Parse(c)).ToArray()));
}

var possibleCombinations = 0;

foreach (var row in rows)
{
    var possibleLines = FindPossibleLines(row.Springs, row.Broken);

    possibleCombinations += possibleLines.Count();
    
    Console.WriteLine($"Row: {row.Springs}, Possible: {possibleLines.Count()}");
}

Console.WriteLine($"Possible combinations: {possibleCombinations}");

IEnumerable<string> FindPossibleLines(string springs, int[] broken)
{
    var possibleLines = new List<string>();

    if (MatchesPattern(springs, broken))
    {
        possibleLines.Add(springs);
    }
    else
    {
        var index = springs.IndexOf('?');

        if (index > -1)
        {
            var replaced = springs.ToCharArray();
            replaced[index] = '#';
            
            possibleLines.AddRange(FindPossibleLines(new string(replaced), broken));
            
            replaced[index] = '.';

            possibleLines.AddRange(FindPossibleLines(new string(replaced), broken));
        }
    }
    
    return possibleLines;
}

bool MatchesPattern(string springs, int[] broken)
{
    var groups = springs.Split('.', StringSplitOptions.RemoveEmptyEntries);

    if (groups.Length == broken.Length)
    {
        for (var i = 0; i < groups.Length; i++)
        {
            if (groups[i].Length != broken[i])
            {
                return false;
            }
        }

        return true;
    }

    return false;
}

record Row(string Springs, int[] Broken);