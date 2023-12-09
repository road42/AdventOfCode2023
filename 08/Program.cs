
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

//Start navigating the nodes
var currentNode = "AAA";
var stepCount = 0;
var instructionPointer = 0;

while (currentNode != "ZZZ")
{
    var direction = instructions[instructionPointer % instructions.Length];
    currentNode = direction == 'L' ? nodes[currentNode].Left : nodes[currentNode].Right;
    instructionPointer++;
    stepCount++;
}

Console.WriteLine($"Result: {stepCount}");


// Ghost trip
// Find all starting nodes (nodes ending with 'A')
var currentNodes = nodes.Keys.Where(n => n.EndsWith("A", StringComparison.CurrentCulture)).ToArray();
var steps = 0;
var startNodes = currentNodes.Length;
var nodesWithZ = 0;
var nextNodes = new List<string>();

do
{
    steps++;
    nextNodes.Clear();

    foreach (var node in currentNodes)
    {
        var direction = instructions[(steps - 1) % instructions.Length];
        var nextNode = direction == 'L' ? nodes[node].Left : nodes[node].Right;
        nextNodes.Add(nextNode);
    }

    nodesWithZ = nextNodes.Count(n => n.EndsWith("Z", StringComparison.CurrentCulture));
    nextNodes.CopyTo(currentNodes);

    if (steps % 100000 == 0)
    {
        Console.WriteLine($"Step {steps}: {currentNodes.Length} nodes {nodesWithZ} with Z");
    }

}
while (nodesWithZ != startNodes);

Console.WriteLine($"All nodes end with Z, stopping at step {steps}");
