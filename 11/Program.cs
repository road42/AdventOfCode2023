var lines = File.ReadAllLines("input.txt");

var rows = lines.Length;
var cols = lines[0].Length;

// Identify empty rows and columns
var emptyRows = new bool[rows];
var emptyCols = new bool[cols];

for (var i = 0; i < rows; i++)
{
    for (var j = 0; j < cols; j++)
    {
        if (lines[i][j] == '#')
        {
            emptyRows[i] = true;
            emptyCols[j] = true;
        }
    }
}

// Calculate new dimensions
var newRows = rows + emptyRows.Count(x => !x);
var newCols = cols + emptyCols.Count(x => !x);
var universe = new char[newRows, newCols];

// Fill the expanded universe
var newRow = 0;
for (var i = 0; i < rows; i++)
{
    if (!emptyRows[i])
    {
        var newCol = 0;
        for (var j = 0; j < cols; j++)
        {
            universe[newRow, newCol] = lines[i][j];
            newCol++;
            if (!emptyCols[j])
            {
                universe[newRow, newCol] = lines[i][j];
                newCol++;
            }
        }
        newRow++;
    }

    var newCol2 = 0;
    for (var j = 0; j < cols; j++)
    {
        universe[newRow, newCol2] = lines[i][j];
        newCol2++;
        if (!emptyCols[j])
        {
            universe[newRow, newCol2] = lines[i][j];
            newCol2++;
        }
    }
    newRow++;
}

// Map Galaxies
var galaxyPositions = new Dictionary<int, (int, int)>();
var galaxyNumber = 1;
var universeRows = universe.GetLength(0);
var universeCols = universe.GetLength(1);

for (var i = 0; i < universeRows; i++)
{
    for (var j = 0; j < universeCols; j++)
    {
        if (universe[i, j] == '#')
        {
            galaxyPositions.Add(galaxyNumber++, (i, j));
        }
    }
}


// Calc sum of shortest paths
var sumOfPaths = 0;
var galaxyNumbers = galaxyPositions.Keys.ToList();

for (var i = 0; i < galaxyNumbers.Count; i++)
{
    for (var j = i + 1; j < galaxyNumbers.Count; j++)
    {
        sumOfPaths += FindShortestPath(universe, galaxyPositions[galaxyNumbers[i]], galaxyPositions[galaxyNumbers[j]]);
    }
}

Console.WriteLine(sumOfPaths);

int FindShortestPath(char[,] universe, (int, int) start, (int, int) end)
{
    if (start == end)
    {
        return 0;
    }

    var visited = new bool[universeRows, universeCols];
    var queue = new Queue<((int, int), int)>();
    queue.Enqueue((start, 0));

    int[] dx = [0, 0, 1, -1];
    int[] dy = [1, -1, 0, 0];

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        (var x, var y) = current.Item1;
        var distance = current.Item2;

        if (x == end.Item1 && y == end.Item2)
        {
            return distance;
        }

        for (var i = 0; i < 4; i++)
        {
            var newX = x + dx[i];
            var newY = y + dy[i];

            if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && !visited[newX, newY])
            {
                visited[newX, newY] = true;
                queue.Enqueue(((newX, newY), distance + 1));
            }
        }
    }

    return -1; // Path not found
}
