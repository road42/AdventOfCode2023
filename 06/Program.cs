using System.Globalization;

var lines = File.ReadAllLines("input.txt");

// Part One: Multiple Races
var raceTimesPartOne = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(part => long.TryParse(part, out _))
                .Select(long.Parse)
                .ToArray();

var recordDistancesPartOne = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(part => long.TryParse(part, out _))
                .Select(long.Parse)
                .ToArray();

long totalWaysPartOne = 1;
for (var i = 0; i < raceTimesPartOne.Length; i++)
{
    var waysToWin = CalculateWaysToWinPart(raceTimesPartOne[i], recordDistancesPartOne[i]);
    totalWaysPartOne *= waysToWin;
}

Console.WriteLine($"Part 1: {totalWaysPartOne}");

// Part Two: Single Long Race
var raceTimePartTwo = long.Parse(new string(lines[0].Where(char.IsDigit).ToArray()), CultureInfo.InvariantCulture);
var recordDistancePartTwo = long.Parse(new string(lines[1].Where(char.IsDigit).ToArray()), CultureInfo.InvariantCulture);
var waysToWinPartTwo = CalculateWaysToWinPart(raceTimePartTwo, recordDistancePartTwo);
Console.WriteLine($"Part 2: {waysToWinPartTwo}");

static long CalculateWaysToWinPart(long raceTime, long recordDistance)
{
    var ways = 0;
    for (var holdTime = 1; holdTime < raceTime; holdTime++)
    {
        var travelTime = raceTime - holdTime;
        var distance = holdTime * travelTime;
        if (distance > recordDistance)
        {
            ways++;
        }
    }
    return ways;
}
