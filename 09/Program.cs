
var lines = File.ReadAllLines("input.txt");
long sumNext = 0;
long sumPrevious = 0;

foreach (var line in lines)
{
    var values = line.Split(' ').Select(long.Parse).ToList();
    sumNext += ExtrapolateValue(values);
    sumPrevious += ExtrapolateValue(values, false);
}

Console.WriteLine($"The sum of the extrapolated (next) values is: {sumNext}");
Console.WriteLine($"The sum of the extrapolated (previous) values is: {sumPrevious}");

static long ExtrapolateValue(List<long> values, bool nextValue = true)
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


    for (var i = sequences.Count - 2; i >= 0; i--)
    {
        // Extrapolate the next value
        if (nextValue)
        {
            sequences[i].Add(sequences[i].Last() + sequences[i + 1].Last());
        }
        else
        {
            sequences[i].Insert(0, sequences[i][0] - sequences[i + 1][0]);
        }
    }

    return nextValue ? sequences[0].Last() : sequences[0][0];
}
