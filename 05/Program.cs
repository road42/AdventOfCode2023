

var lines = File.ReadAllLines("input.txt");
var initialSeeds = lines[0].Substring(7).Trim().Split(' ').Select(long.Parse).ToList();
var mappings = ParseMappings(lines);
var lowestLocation = initialSeeds.Select(seed => ConvertThroughMappings(seed, mappings))
                                         .Min();

Console.WriteLine(lowestLocation);

static Dictionary<string, Dictionary<long, long>> ParseMappings(string[] lines)
{
    var mappings = new Dictionary<string, Dictionary<long, long>>();
    var currentMappingKey = "";

    foreach (var line in lines.Skip(1))
    {
        if (string.IsNullOrWhiteSpace(line))
            continue;

        if (line.EndsWith("map:", StringComparison.OrdinalIgnoreCase))
        {
            currentMappingKey = line.Split(' ')[0];
            mappings[currentMappingKey] = [];
        }
        else
        {
            var parts = line.Split(' ').Select(long.Parse).ToArray();
            for (var i = 0; i < parts[2]; i++)
            {
                mappings[currentMappingKey][parts[1] + i] = parts[0] + i;
            }
        }
    }

    return mappings;
}

static long ConvertThroughMappings(long seed, Dictionary<string, Dictionary<long, long>> mappings)
{
    var currentNumber = seed;

    foreach (var mapping in mappings)
    {
        currentNumber = Convert(currentNumber, mapping.Value);
    }

    return currentNumber;
}

static long Convert(long source, Dictionary<long, long> map) => map.TryGetValue(source, out var value) ? value : source;
