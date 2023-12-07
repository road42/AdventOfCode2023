using System.Globalization;

var lines = File.ReadAllLines("input.txt");
var hands = new List<Hand>();

// read hands
foreach (var line in lines)
{
    var parts = line.Split(' ');
    hands.Add(new Hand(parts[0], int.Parse(parts[1], CultureInfo.InvariantCulture)));
}

var rank = 1;
// order by hand type
foreach (var hand in hands
                        .OrderByDescending(h => h.HandType)
                        .ThenByDescending(h => h.HandValues[0])
                        .ThenByDescending(h => h.HandValues[1])
                        .ThenByDescending(h => h.HandValues[2])
                        .ThenByDescending(h => h.HandValues[3])
                        .ThenByDescending(h => h.HandValues[4])
                        .Reverse()
                        )
{
    hand.Rank = rank++;
    Console.WriteLine(hand.ToString());
}

Console.WriteLine(hands.Sum(h => h.Won));


internal sealed class Hand(string cards, int bid)
{
    public string Cards { get; set; } = cards;
    public int Bid { get; set; } = bid;
    public int Rank { get; set; }
    public int HandType => GetHandType(this.Cards);
    public int Won => this.Bid * this.Rank;

    public override string ToString() => $"C:{this.Cards} T:{this.HandType} R:{this.Rank} B:{this.Bid} V:{this.HandValues.Select(v => v.ToString(CultureInfo.InvariantCulture)).Aggregate((a, b) => $"{a},{b}")}";

    public int[] HandValues => this.Cards
                                    //.Distinct()
                                    //.OrderByDescending(c => CardValues[c])
                                    .Select(c => CardValues[c])
                                    .ToArray();

    private static int GetHandType(string hand)
    {
        var cardCounts = hand.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
        var cardGroups = cardCounts.Values.OrderByDescending(v => v).ToList();

        var handTypes = new Dictionary<List<int>, int>
        {
            { new List<int> { 5 }, 7 },       // Five of a kind
            { new List<int> { 4, 1 }, 6 },    // Four of a kind
            { new List<int> { 3, 2 }, 5 },    // Full house
            { new List<int> { 3, 1, 1 }, 4 }, // Three of a kind
            { new List<int> { 2, 2, 1 }, 3 }, // Two pair
            { new List<int> { 2, 1, 1, 1 }, 2 } // One pair
        };

        foreach (var handType in handTypes)
        {
            if (cardGroups.SequenceEqual(handType.Key))
            {
                return handType.Value;
            }
        }

        return 1; // High card
    }

    private static readonly Dictionary<char, int> CardValues = new()
    {
        {'A', 14}, {'K', 13}, {'Q', 12}, {'J', 11}, {'T', 10},
        {'9', 9}, {'8', 8}, {'7', 7}, {'6', 6}, {'5', 5},
        {'4', 4}, {'3', 3}, {'2', 2}
    };
}
