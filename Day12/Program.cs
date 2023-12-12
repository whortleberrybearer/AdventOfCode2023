var input = File.ReadLines("Input.txt");
var rows = new List<Row>();

foreach (var line in input)
{
    Console.WriteLine(line);
    
    var parts = line.Split(' ');

    var springs = parts[0];
    var damaged = parts[1];

    //springs = string.Join('?', Enumerable.Repeat(springs, 5));
    //damaged = string.Join(',', Enumerable.Repeat(damaged, 5));

    rows.Add(new Row(springs, damaged.Split(',').Select(c => int.Parse(c)).ToArray()));
}

var possibleCombinations = 0l;

foreach (var row in rows)
{
    
    var possibleLines1 = FindPossibleLines(row.Springs, row.Broken).Count();
    var possibleLines2 = FindPossibleLines(row.Springs + "?" + row.Springs, row.Broken.Concat(row.Broken).ToArray()).Count();

    var combinations = (long)Math.Pow((long)(possibleLines2 / possibleLines1), 4) * possibleLines1;
    // Long time, too low.  19617238612177
    
    possibleCombinations += combinations;

    Console.WriteLine($"Row: {row.Springs}, Possible: {combinations}");
    Console.WriteLine();
}

Console.WriteLine($"Possible combinations: {possibleCombinations}");

IEnumerable<string> FindPossibleLines(string springs, int[] broken)
{
    Console.WriteLine(springs);
    var possibleLines = new List<string>();

    if (MatchesPattern(springs, broken))
    {
        possibleLines.Add(springs);
    }
    else
    {
        var index = springs.IndexOf('?');

        // Early escape clause.  If the formed blocks do not meet the patter so far, stop checking.
        if ((index > -1) && PartialMatchesPattern(springs.Remove(index), broken))
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

bool PartialMatchesPattern(string springs, int[] broken)
{
    var groups = springs.Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();

    // If the last character isn't a '.', then its a partial group, so ignore it.
    if (!springs.EndsWith(".") && (groups.Count > 0))
    {
        groups.RemoveAt(groups.Count - 1);
    }

    if (groups.Count <= broken.Length)
    {
        // Don't check the last group as it may not be fully formed.
        for (var i = 0; i < groups.Count(); i++)
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