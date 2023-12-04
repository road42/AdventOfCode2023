using System.Globalization;

var lines = File.ReadAllLines("input.txt");
var scratchCards = new List<ScratchCard>();

foreach (var line in lines)
{
    scratchCards.Add(new ScratchCard(line));
}

var totalPoints = scratchCards.Sum(c => c.CalculatePoints());

Console.WriteLine(totalPoints);

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
}
