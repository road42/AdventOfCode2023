#pragma warning disable CA1310 // Specify StringComparison for correctness

var lines = File.ReadAllLines("input.txt");
var amountOfMaps = 7;

// read initial seeds
var seeds = lines[0][7..].Split(' ').Select(long.Parse).ToArray();
var leastSeed = long.MaxValue;

// Part 1
foreach (var seed in seeds)
{
    var location = getNumForSource(seed, 1);
    leastSeed = location < leastSeed ? location : leastSeed;
}

Console.WriteLine($"Least Seed (Part 1): {leastSeed}");

// Part 2
// Reset
leastSeed = long.MaxValue;

for (var i = 0; i < seeds.Length; i += 2)
{

    var seed = seeds[i];
    var range = seeds[i + 1];

    Console.WriteLine($"    - Seed (Part 2): {seed} {range}");

    // do the hard way
    // generate subseeds
    for (var j = seed; j < seed + range; j++)
    {
        var location = getNumForSource(j, 1);
        leastSeed = location < leastSeed ? location : leastSeed;
    }
}

Console.WriteLine($"Least Seed (Part 2): {leastSeed}");

// get the "soil" for the "seed" (map no 1)
long getNumForSource(long sourceNumber, int map)
{
    var loopMap = 0;
    var destinationNumber = sourceNumber;

    foreach (var line in lines.Skip(1))
    {
        if (string.IsNullOrWhiteSpace(line))
            continue;

        if (line.EndsWith("map:"))
        {
            loopMap++;
            continue;
        }

        if (loopMap > map)
            break;

        // found the map
        if (loopMap == map)
        {
            // look for range
            var address = line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();

            // is in range?
            var destinationMapNumber = address[0];
            var sourceMapNumber = address[1];
            var range = address[2];

            // is in source range?
            if (sourceNumber >= sourceMapNumber && sourceNumber < sourceMapNumber + range)
            {
                // calculate destination number
                destinationNumber = destinationMapNumber + (sourceNumber - sourceMapNumber);
                break;
            }
        }
    }

    if (map < amountOfMaps)
    {
        destinationNumber = getNumForSource(destinationNumber, ++map);
    }

    return destinationNumber;
}
