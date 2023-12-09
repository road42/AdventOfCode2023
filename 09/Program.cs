
var lines = File.ReadAllLines("input.txt");
long sum = 0;

foreach (var line in lines)
{
    var values = line.Split(' ').Select(long.Parse).ToList();
    sum += ExtrapolateNextValue(values);
}

Console.WriteLine($"The sum of the extrapolated values is: {sum}");

static long ExtrapolateNextValue(List<long> values)
{
    var sequences = new List<List<long>> { new(values) };

    // Generate sequences of differences
    while (sequences.Last().Distinct().Count() > 1 || sequences.Last().First() != 0)
    {
        var lastSequence = sequences.Last();
        var nextSequence = new List<long>();

        for (var i = 0; i < lastSequence.Count - 1; i++)
        {
            nextSequence.Add(lastSequence[i + 1] - lastSequence[i]);
        }

        sequences.Add(nextSequence);
    }

    // Extrapolate the next value
    for (var i = sequences.Count - 2; i >= 0; i--)
    {
        sequences[i].Add(sequences[i].Last() + sequences[i + 1].Last());
    }

    return sequences[0].Last();
}
