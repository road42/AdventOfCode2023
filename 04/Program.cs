using System.Globalization;

// Read the file and create a list of scratch cards
var scratchCards = File
    .ReadAllLines("input.txt")
    .Select(line => new ScratchCard(line))
    .ToList();

// Part 1: incl. calc
var totalPoints = scratchCards.Sum(c => c.CalculatePoints());
Console.WriteLine(totalPoints);

// Part 2
var processQueue = new Queue<ScratchCard>(scratchCards);
var part2Sum = processQueue.Count;

while (processQueue.Count != 0)
{
    var card = processQueue.Dequeue();
    part2Sum += card.MatchingNumbers;

    for (var i = 0; i < card.MatchingNumbers; i++)
    {
        processQueue.Enqueue(scratchCards.First(c => c.Id == (card.Id + i + 1)));
    }
}

Console.WriteLine(part2Sum);


internal sealed class ScratchCard
{
    public ScratchCard(string line)
    {
        // Example: Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
        var parts = line.Replace("  ", " ").Split(":");
        this.Id = int.Parse(parts[0].Replace("Card ", ""), CultureInfo.InvariantCulture);
        this.WinningNumbers = parts[1].Split(" | ")[0].Trim().Split(" ").Select(int.Parse).ToList();
        this.YourNumbers = parts[1].Split(" | ")[1].Trim().Split(" ").Select(int.Parse).ToList();
    }

    public int Id { get; set; }
    public List<int> WinningNumbers { get; set; } = [];
    public List<int> YourNumbers { get; set; } = [];

    public int CalculatePoints()
    {
        var points = 0;

        foreach (var number in this.YourNumbers)
        {
            if (this.WinningNumbers.Contains(number))
            {
                points = points == 0 ? 1 : points * 2;
            }
        }
        return points;
    }

    public int MatchingNumbers
    {
        get
        {
            var matchingNumbers = new List<int>();

            foreach (var number in this.YourNumbers)
            {
                if (this.WinningNumbers.Contains(number))
                {
                    matchingNumbers.Add(number);
                }
            }
            return matchingNumbers.Count;
        }
    }
}
