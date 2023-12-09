using System.Numerics;

var lines = File.ReadAllLines("input.txt");
var instructions = lines[0];

// The rest of the lines contain the node mappings
var nodes = new Dictionary<string, (string Left, string Right)>();

for (var i = 1; i < lines.Length; i++)
{
    if (string.IsNullOrWhiteSpace(lines[i]))
    {
        continue;
    }

    var parts = lines[i].Split(" = ");
    var node = parts[0];
    var connections = parts[1].Trim(['(', ')']).Split(", ");
    nodes[node] = (connections[0], connections[1]);
}

// Part 1
string[] part1StartNodes = { "AAA" };
Console.WriteLine($"Result Part 1: {nodeSteps(part1StartNodes, "ZZZ").Sum()}");

// Part 2
// GhostMode
// Very hard, had to rework the whole thing (and needed help :-))
// New way:
// - There are 6 start and end nodes
// - Calculate the number for every node
var part2StartNodes = nodes.Keys.Where(n => n.EndsWith('A')).ToArray();
var stepArray = nodeSteps(part2StartNodes, "Z");
Console.WriteLine($"Result Part 2: {stepArray.Aggregate(LCM)}");

// Generic funtion to calculate steps
long[] nodeSteps(string[] startNodes, string endMarker)
{
    var seen = new List<long>();

    foreach (var node in startNodes)
    {
        var steps = 0;
        var currentNode = node;
        var instructionPointer = 0;

        while (!currentNode.EndsWith(endMarker, StringComparison.CurrentCulture))
        {
            var direction = instructions[instructionPointer % instructions.Length];
            currentNode = direction == 'L' ? nodes[currentNode].Left : nodes[currentNode].Right;
            instructionPointer++;
            steps++;
        }

        seen.Add(steps);
    }

    return [.. seen];
}

// Calc the lowest common multiple for the values of the different graphs
static long LCM(long a, long b) => (long)((BigInteger)a * (BigInteger)b / BigInteger.GreatestCommonDivisor((BigInteger)a, (BigInteger)b));
