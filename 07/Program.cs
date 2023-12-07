using System.Globalization;

var lines = File.ReadAllLines("input.txt");
var handsPart1 = new List<HandPart1>();
var handsPart2 = new List<HandPart2>();

// read hands
foreach (var line in lines)
{
    var parts = line.Split(' ');
    handsPart1.Add(new HandPart1(parts[0], int.Parse(parts[1], CultureInfo.InvariantCulture)));
    handsPart2.Add(new HandPart2(parts[0], int.Parse(parts[1], CultureInfo.InvariantCulture)));
}

OutputHands([.. handsPart1]);
OutputHands([.. handsPart2]);

static void OutputHands(List<BaseHand> hands)
{
    var rank = 1;
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
}



internal sealed class HandPart1(string cards, int bid) : BaseHand(cards, bid)
{
    public override int HandType => GetHandType(this.Cards);

    public override int[] HandValues => this.Cards
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

internal sealed class HandPart2(string cards, int bid) : BaseHand(cards, bid)
{
    public override int HandType => GetHandType(this.Cards);

    public override int[] HandValues => this.Cards
                                    //.Distinct()
                                    //.OrderByDescending(c => CardValues[c])
                                    .Select(c => CardValues[c])
                                    .ToArray();

    private static int GetHandType(string hand)
    {
        var jokerCount = hand.Count(c => c == 'J');
        var cardCounts = hand.Where(c => c != 'J').GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
        var cardGroups = cardCounts.Values.OrderByDescending(v => v).ToList();

        // add the joker count to the value of the first entry in cardGroups
        if (cardGroups.Count > 0)
        {
            cardGroups[0] += jokerCount;
        }
        else
        {
            // should be "JJJJJ"
            cardGroups.Add(jokerCount);
        }

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
        {'A', 14}, {'K', 13}, {'Q', 12}, {'J', 1}, {'T', 10},
        {'9', 9}, {'8', 8}, {'7', 7}, {'6', 6}, {'5', 5},
        {'4', 4}, {'3', 3}, {'2', 2}
    };
}

internal abstract class BaseHand(string cards, int bid)
{
    public string Cards { get; set; } = cards;
    public int Bid { get; set; } = bid;
    public int Rank { get; set; }
    public abstract int HandType { get; }

    public int Won => this.Bid * this.Rank;

    public override string ToString() => $"C:{this.Cards} T:{this.HandType} R:{this.Rank} B:{this.Bid} V:{this.HandValues.Select(v => v.ToString(CultureInfo.InvariantCulture)).Aggregate((a, b) => $"{a},{b}")}";

    public abstract int[] HandValues { get; }
}
