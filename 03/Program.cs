
using System.Globalization;

var lines = File.ReadAllLines("input.txt");
var sumPart1 = 0;
var sumPart2 = 0;

var gears = new Dictionary<string, List<int>>();

// Scan for numbers
for (var i = 0; i < lines.Length; i++)
{
    for (var j = 0; j < lines[i].Length;)
    {
        // find first digit in line
        if (char.IsDigit(lines[i][j]))
        {
            // find last digit for number and save coordinates
            var startCol = j;
            while (j < lines[i].Length && char.IsDigit(lines[i][j]))
            {
                j++;
            }

            var number = int.Parse(lines[i].AsSpan(startCol, j - startCol), CultureInfo.InvariantCulture);

            // look for adjacent symbols from the first digit to the last digit
            if (IsAdjacentToSymbol(lines, i, startCol, j - startCol, number, gears))
            {
                sumPart1 += number;
            }
        }
        else
        {
            j++;
        }
    }
}

static bool IsAdjacentToSymbol(string[] schematic, int row, int startCol, int length, int number, Dictionary<string, List<int>> gears)
{
    // look around the number for adjacent symbols
    for (var col = startCol; col < startCol + length; col++)
    {
        // the delta coordinates for the adjacent cells
        int[] dx = { -1, -1, -1, 0, 1, 1, 1, 0 };
        int[] dy = { -1, 0, 1, 1, 1, 0, -1, -1 };

        for (var i = 0; i < 8; i++)
        {
            // position to look for
            var newRow = row + dy[i];
            var newCol = col + dx[i];

            // look only inside the schematic
            if (newRow >= 0 && newRow < schematic.Length && newCol >= 0 && newCol < schematic[0].Length)
            {
                var adjacentChar = schematic[newRow][newCol];
                if (adjacentChar != '.' && !char.IsDigit(adjacentChar))
                {
                    // part2: if adjacent symbol is a gear
                    if (adjacentChar == '*')
                    {
                        //generate a symbol id (x+y) and add every number found around it to a list
                        var gearId = $"{newRow}-{newCol}";
                        gears[gearId] = gears.TryGetValue(gearId, out var value) ? value : [];
                        gears[gearId].Add(number);
                    }

                    return true;
                }
            }
        }
    }

    return false;
}

// add up part2
sumPart2 += gears
    .Where(symbol => symbol.Value.Count == 2)
    .Sum(symbol => symbol.Value.Aggregate(1, (acc, x) => acc * x));

Console.WriteLine(sumPart1);
Console.WriteLine(sumPart2);
