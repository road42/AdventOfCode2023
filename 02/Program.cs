
var lines = File.ReadAllLines("input.txt");

var maxColors = new Dictionary<string, int>()
{
    { "red", 12 },
    { "green", 13 },
    { "blue", 14 }
};

var sum = 0;
var sumPart2 = 0;

foreach (var game in lines)
{
    var gameIsOk = true;

    // split the ":" and get the game number
    var gameString = game.Split(':')[0].Replace("Game", "");
    var gameNumber = int.Parse(gameString, System.Globalization.CultureInfo.InvariantCulture);

    // process games
    var roundsList = game.Split(':')[1].Split(';');

    var maxColorsGame = new Dictionary<string, int>()
    {
        { "red", 0 },
        { "green", 0 },
        { "blue", 0 }
    };

    foreach (var round in roundsList)
    {
        foreach (var colorString in round.Split(','))
        {
            var colorParts = colorString.Trim().Split(' ');
            var color = colorParts[1].Trim();
            var number = int.Parse(colorParts[0], System.Globalization.CultureInfo.InvariantCulture);

            // Part 1: check if the color is valid
            if (maxColors.TryGetValue(color, out var maxColor) && maxColor < number)
                gameIsOk = false;

            // Part 2: save the max number of each color
            if (maxColorsGame.TryGetValue(color, out var maxColorGame) && maxColorGame < number)
                maxColorsGame[color] = number;
        }
    }

    // Part 1: add the game number to the sum
    if (gameIsOk)
        sum += gameNumber;

    // Part 2: multiply the max numbers of each color (> 0)
    sumPart2 += maxColorsGame.Values.Where(x => x > 0).Aggregate(1, (acc, x) => acc * x);
}

Console.WriteLine(sum);
Console.WriteLine(sumPart2);

