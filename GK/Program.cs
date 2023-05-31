using GK;
using GK.Strategy;


int n;
int k;
int c;

while (true)
{
    Console.WriteLine("Please input the following parameters: n k c");

    var data = Console.ReadLine();
    var splittedData = data?.Split(' ');
    if (splittedData?.Length != 3)
    {
        Console.WriteLine("Incorrect number of parameters.");
        continue;
    }

    if (!int.TryParse(splittedData[0], out n) || n < 1)
    {
        Console.WriteLine("Please input a valid value for parameter n\n");
        continue;
    }

    if (!int.TryParse(splittedData[1], out k) || k < 1)
    {
        Console.WriteLine("Please input a valid value for parameter k\n");
        continue;
    }

    if (!int.TryParse(splittedData[2], out c) || c < 1)
    {
        Console.WriteLine("Please input a valid value for parameter c\n");
        continue;
    }

    break;
}

IStrategy player2Strategy;

while (true)
{
    Console.WriteLine("Please input player 2 strategy:");
    Console.WriteLine("1: edge subsequence elements");
    Console.WriteLine("2: elements destroying the most amount of subsequences");

    var strategyReadLine = Console.ReadLine();
    switch (strategyReadLine)
    {
        // first strategy
        case "1":
            player2Strategy = new Strategy1(n, k, c);
            break;

        // second strategy
        case "2":
            player2Strategy = new Strategy2(n, k, c);
            break;

        // invalid input
        default:
            Console.WriteLine("Please enter a valid strategy number.");
            continue;
    }

    break;
}

var game = new Game(n, k, c, player2Strategy);
game.PlayHuman();