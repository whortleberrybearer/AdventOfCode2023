var input = File.ReadLines("Input.txt");
var hashTotal = 0;
foreach (var line in input)
{
    Console.WriteLine(line);

    foreach (var part in line.Split(','))
    {
        var hash = CalculateHash(part);

        Console.WriteLine($"Part: {part}, Hash: {hash}");

        hashTotal += hash;
    }
}

Console.WriteLine($"Hash total: {hashTotal}");

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