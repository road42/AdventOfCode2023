using System.Globalization;

var lines = File.ReadAllLines("input.txt");
var sum = 0;

var digitMap = new Dictionary<string, int>
{
    { "one",   1 },
    { "two",   2 },
    { "three", 3 },
    { "four",  4 },
    { "five",  5 },
    { "six",   6 },
    { "seven", 7 },
    { "eight", 8 },
    { "nine",  9 }
};

foreach (var line in lines)
{
    var newLine = "";

    // step through each character in the line
    for (var i = 0; i < line.Length; i++)
    {
        // if the character is a digit, add it to the new line
        if (char.IsDigit(line[i]))
        {
            newLine += line[i];
        }
        else
        {
            // otherwise, check if the next few characters are a digit from the map
            foreach (var (chars, value) in digitMap)
            {
                if (line[i..].StartsWith(chars, StringComparison.InvariantCulture))
                {
                    newLine += value;
                }
            }
        }
    }

    // find the first and last digit in the line
    var firstDigit = newLine.FirstOrDefault(char.IsDigit);
    var lastDigit = newLine.LastOrDefault(char.IsDigit);

    // combine the two digits into a number
    var number = int.Parse(firstDigit.ToString() + lastDigit.ToString(), CultureInfo.InvariantCulture);
    sum += number;
}

Console.WriteLine(sum);
