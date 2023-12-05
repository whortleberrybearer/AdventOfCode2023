var input = File.ReadAllText("Input.txt");
var lines = input.Split("\r\n");
var total = 0;
var totalGearRatio = 0;

for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
{
    var line = lines[lineIndex];

    Console.WriteLine($"Line: {line}");

    var numberStartIndex = -1;
    var numberEndIndex = -1;

    for (int characterIndex = 0; characterIndex < line.Length; characterIndex++)
    {
        // Part 1 needs numbers adjacent to symbols.
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

        // Part 2 needs 2 numbers adjacent to a *.
        if (line[characterIndex] == '*')
        {
            var numbers = new List<int>();

            if (lineIndex > 0)
            {
                numbers.AddRange(FindNumbers(lines[lineIndex - 1], characterIndex));
            }

            numbers.AddRange(FindNumbers(lines[lineIndex], characterIndex));

            if (lineIndex < lines.Length - 1)
            {
                numbers.AddRange(FindNumbers(lines[lineIndex + 1], characterIndex));
            }

            if (numbers.Count == 2)
            {
                var gearRatio = numbers[0] * numbers[1];

                Console.WriteLine($"Gear ratio: {numbers[0]} * {numbers[1]} = {gearRatio}");

                totalGearRatio += gearRatio;
            }
        }
    }

    Console.WriteLine();
}

Console.WriteLine($"Total: {total}");
Console.WriteLine($"Total gear ration: {totalGearRatio}");

IEnumerable<int> FindNumbers(string line, int multiplierIndex)
{
    var numbers = new List<int>();
    var numberStartIndex = -1;
    var numberEndIndex = -1;
    var left = Math.Max(0, multiplierIndex - 1);
    var rigth = Math.Min(line.Length - 1, multiplierIndex + 1);

    for (int characterIndex = 0; characterIndex < line.Length; characterIndex++)
    {
        // Part 1 needs numbers adjacent to symbols.
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
         
            // Now check if that number is within the bounds of the multiplier.
            if ((numberStartIndex <= rigth) && (numberEndIndex >= left))
            {
                numbers.Add(number);
            }

            // Need to reset the index for the next number.
            numberStartIndex = -1;
            numberEndIndex = -1;
        }
    }

    return numbers;
}
