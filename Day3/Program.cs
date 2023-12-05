var input = File.ReadAllText("Input.txt");
var lines = input.Split("\r\n");
var total = 0;

for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
{
    var line = lines[lineIndex];

    Console.WriteLine($"Line: {line}");

    var numberStartIndex = -1;
    var numberEndIndex = -1;

    for (int characterIndex = 0; characterIndex < line.Length; characterIndex++)
    {
        if (char.IsNumber(line[characterIndex]))
        {
            if (numberStartIndex == -1)
            {
                numberStartIndex = characterIndex;
            }

            numberEndIndex = characterIndex;
        }

        if (((numberEndIndex != -1) && (numberEndIndex != characterIndex)) || (numberEndIndex == line.Length - 1))
        {
            // Found the end of a number
            var number = int.Parse(line.Substring(numberStartIndex, numberEndIndex - numberStartIndex + 1));

            var top = Math.Max(0, lineIndex - 1);
            var bottom = Math.Min(lines.Length - 1, lineIndex + 1);
            var left = Math.Max(0, numberStartIndex - 1);
            var right = Math.Min(line.Length - 1, numberEndIndex + 1);
            var bounds =
                lines[top].Substring(left, right - left + 1) +
                lines[lineIndex].Substring(left, right - left + 1) +
                lines[bottom].Substring(left, right - left + 1);
            var hasSymbol = bounds.Any(c => !char.IsNumber(c) && c != '.');

            if (hasSymbol)
            {
                Console.WriteLine($"Number: {number} has symbol");

                total += number;
            }
            else
            {
                Console.WriteLine($"Number: {number} does not have symbol");
            }

            // Need to reset the index for the next number.
            numberStartIndex = -1;
            numberEndIndex = -1;
        }
    }

    Console.WriteLine();
}

Console.WriteLine($"Total: {total}");