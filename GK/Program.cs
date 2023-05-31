using GK;
using GK.Strategy;


int n;
int k;
int c;

while (true)
{
    Console.WriteLine("Podaj dane wejściowe: n k c");

    var data = Console.ReadLine();
    var splittedData = data?.Split(' ');
    if (splittedData?.Length != 3)
    {
        Console.WriteLine("Nieprawidłowa liczba argumentów.");
        continue;
    }

    if (!int.TryParse(splittedData[0], out n) || n < 1)
    {
        Console.WriteLine("Nieprawidłowa wartość parametru n\n");
        continue;
    }

    if (!int.TryParse(splittedData[1], out k) || k < 1)
    {
        Console.WriteLine("Nieprawidłowa wartość parametru k\n");
        continue;
    }

    if (!int.TryParse(splittedData[2], out c) || c < 1)
    {
        Console.WriteLine("Nieprawidłowa wartość parametru c\n");
        continue;
    }

    break;
}

IStrategy player2Strategy;

while (true)
{
    Console.WriteLine("Wybierz strategię gracza 2:");
    Console.WriteLine("1)");
    Console.WriteLine("2)");

    var strategyReadLine = Console.ReadLine();
    switch (strategyReadLine)
    {
        // first strategy
        case "1":
        case "1)":
            player2Strategy = new Strategy1(n, k, c);
            break;

        // second strategy
        case "2":
        case "2)":
            player2Strategy = new Strategy2(n, k, c);
            break;

        // invalid input
        default:
            Console.WriteLine("Źle wybrana strategia");
            continue;
    }

    break;
}

var game = new Game(n, k, c, player2Strategy);
game.PlayHuman();