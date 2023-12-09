
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

// Start navigating the nodes
var currentNode = "AAA";
var stepCount = 0;
var instructionPointer = 0;

while (currentNode != "ZZZ")
{
    Console.WriteLine($"{currentNode} {stepCount}");
    var direction = instructions[instructionPointer % instructions.Length];
    currentNode = direction == 'L' ? nodes[currentNode].Left : nodes[currentNode].Right;
    instructionPointer++;
    stepCount++;
}

Console.WriteLine($"Result: {stepCount}");
