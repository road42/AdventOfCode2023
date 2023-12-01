using System.Globalization;

var lines = File.ReadAllLines("input.txt");
var sum = 0;

foreach (var line in lines)
{
    // find the first and last digit in the line
    var firstDigit = line.FirstOrDefault(char.IsDigit);
    var lastDigit = line.LastOrDefault(char.IsDigit);

    // combine the two digits into a number
    var number = int.Parse(firstDigit.ToString() + lastDigit.ToString(), CultureInfo.InvariantCulture);
    sum += number;
}

Console.WriteLine(sum);

