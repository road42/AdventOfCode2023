// parse input
var lines = File.ReadAllLines("input.txt");
var grid = lines.Select(line => line.ToCharArray()).ToArray();
var rows = grid.Length;
var cols = grid[0].Length;

// find start position
var startPosition = (0, 0);

for (var i = 0; i < rows; i++)
{
    for (var j = 0; j < cols; j++)
    {
        if (grid[i][j] == 'S')
        {
            startPosition = (i, j);
        }
    }
}

var visited = new HashSet<(int, int)>
{
    startPosition
};


// Find the loop tiles
Console.WriteLine("Start position: " + startPosition);

var queue = new Queue<(int, int)>();
queue.Enqueue(startPosition);

while (queue.Count > 0)
{
    var nextPosition = queue.Dequeue();

    foreach (var nextTile in GetAllTiles(nextPosition))
    {
        queue.Enqueue(nextTile);
    }
}

// walk the loop
// queue.Enqueue(startPosition);
// var distance = 0;
// var lastPosition = (-1, -1);

// while (queue.Count > 0)
// {
//     var nextPosition = queue.Dequeue();
//     var nextTile = WalkTheLine(lastPosition, nextPosition);
// }

// print result
foreach (var position in visited)
{
    Console.WriteLine($"Visited: {position.Item1} {position.Item2} {grid[position.Item1][position.Item2]}");
}
Console.WriteLine($"Amount: {Math.Ceiling(visited.Count/2.0)}");


// get all tiles from the loop
(int, int)[] GetAllTiles((int, int) position)
{
    var current = position;
    var currentTile = grid[current.Item1][current.Item2];
    var directions = new List<(int, int)>();
    var nextPositions = new List<(int, int)>();

    if (currentTile == 'S')
    {
        directions.Add((0, 1));
        directions.Add((0, -1));
        directions.Add((1, 0));
        directions.Add((-1, 0));
    }
    else if (currentTile == '|')
    {
        directions.Add((-1, 0));
        directions.Add((1, 0));
    }
    else if (currentTile == '-')
    {
        directions.Add((0, -1));
        directions.Add((0, 1));
    }
    else if (currentTile == 'L')
    {
        directions.Add((0, 1));
        directions.Add((-1, 0));
    }
    else if (currentTile == 'J')
    {
        directions.Add((0, -1));
        directions.Add((-1, 0));
    }
    else if (currentTile == '7')
    {
        directions.Add((0, -1));
        directions.Add((1, 0));
    }
    else if (currentTile == 'F')
    {
        directions.Add((0, 1));
        directions.Add((1, 0));
    }

    foreach (var direction in directions)
    {
        var nextRow = current.Item1 + direction.Item1;
        var nextCol = current.Item2 + direction.Item2;

        if (nextRow < 0 || nextRow >= rows || nextCol < 0 || nextCol >= cols)
        {
            continue;
        }

        var nextTile = grid[nextRow][nextCol];

        if (ValidNextTileFor(direction, currentTile).Contains(nextTile) && !visited.Contains((nextRow, nextCol)))
        {
            nextPositions.Add((nextRow, nextCol));
        }
    }

    _ = visited.Add(current);

    return [.. nextPositions];
}

string ValidNextTileFor((int, int) direction, char tile)
{
    // This is ugly but it works

    if (tile == 'S' && direction == (-1, 0))
    {
        // up
        return "|F7";
    }
    else if (tile == 'S' && direction == (1, 0))
    {
        // down
        return "LJ|";
    }
    else if (tile == 'S' && direction == (0, -1))
    {
        // left
        return "-LF";
    }
    else if (tile == 'S' && direction == (0, 1))
    {
        // right
        return "-7J";
    }
    else if (tile == '|' && direction == (-1, 0))
    {
        // up
        return "F7|";
    }
    else if (tile == '|' && direction == (1, 0))
    {
        // down
        return "LJ|";
    }
    else if (tile == '-' && direction == (0, -1))
    {
        // left
        return "FL-";
    }
    else if (tile == '-' && direction == (0, 1))
    {
        // right
        return "7J-";
    }
    else if (tile == 'F' && direction == (0, 1))
    {
        // right
        return "-7J";
    }
    else if (tile == 'F' && direction == (1, 0))
    {
        // down
        return "|LJ";
    }
    else if (tile == '7' && direction == (0, -1))
    {
        // left
        return "-FL";
    }
    else if (tile == '7' && direction == (1, 0))
    {
        // down
        return "|JL";
    }
    else if (tile == 'J' && direction == (-1, 0))
    {
        // up
        return "|F7";
    }
    else if (tile == 'J' && direction == (0, -1))
    {
        // left
        return "-FL";
    }
    else if (tile == 'L' && direction == (0, 1))
    {
        // right
        return "-J7";
    }
    else if (tile == 'L' && direction == (-1, 0))
    {
        // up
        return "|F7";
    }

    return "";
}
