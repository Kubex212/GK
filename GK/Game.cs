using GK.Strategy;

namespace GK
{
    public class Game
    {
        private readonly int c;
        private readonly int k;
        private readonly int n;

        private readonly IStrategy player2Strategy;

        private int[] numbers;
        private List<int[]> subsequences;
        private List<int[]> T;

        private enum MakeMoveResult
        {
            NoOneWon,
            Player1Won,
            Player2Won
        };

        public Game(int n, int k, int c, IStrategy player2Strategy)
        {
            this.n = n;
            this.k = k;
            this.c = c;

            this.player2Strategy = player2Strategy;

            numbers = new int[n];
            subsequences = Utility.GetAllSubsequences(n, k);

            T = new List<int[]>();
            for (var i = 0; i < subsequences.Count; i++)
                T.Add(new int[c + 1]);
        }

        public void PlayHuman()
        {
            DisplayState();
            while (true)
            {
                try
                {
                    Console.Write("Ruch gracza 1. Wpisz ruch: {liczba} {kolor}\n");
                    var line = Console.ReadLine()!;
                    var num = int.Parse(line.Split()[0]);
                    var col = int.Parse(line.Split()[1]);

                    if (num < 1 || num > n)
                    {
                        Console.WriteLine("Podaj poprawną liczbę od 1 do n.");
                        throw new Exception("zla liczba");
                    }
                    if (col < 1 || col > k)
                    {
                        Console.WriteLine("Podaj poprawny kolor od 1 do k.");
                        throw new Exception("zly kolor");
                    }

                    MakeMove(null, player2Strategy, 1, true, num - 1, col);
                    //ConsoleExtension.ClearLine();

                    //Console.WriteLine("Ruch gracza 2.");
                    //Console.ReadKey(true);
                    //ConsoleExtension.ClearLine();
                    if (MakeMove(player2Strategy, null, 2, true) != MakeMoveResult.NoOneWon)
                        return;
                }
                catch (Exception) { }

            }
        }

        private MakeMoveResult MakeMove(IStrategy playingStrategy, IStrategy notPlayingStrategy, int playingPlayer, bool demo, int? num = null, int? col = null)
        {
            var (number, color) = (0, 0);
            if (playingStrategy != null)
            {
                (number, color) = playingStrategy.MakeMove(numbers);
                if (numbers[number] != 0)
                    throw new InvalidOperationException($"Tried to recolor number {number} from color {numbers[number]} to color {color}.");
            }
            else
            {
                (number, color) = (num!.Value, col!.Value);
                if (numbers[number] != 0)
                    throw new InvalidOperationException($"Tried to recolor number {number} from color {numbers[number]} to color {color}.");
                // notPlayingStrategy is not null here
                notPlayingStrategy.Update(number, color);
            }


            Update(number, color);

            if (demo)
            {
                DisplayMove(playingPlayer, number, color);
                DisplayState();
            }

            if (Player1Won())
            {
                if (demo)
                {
                    Console.Write("Wygrana gracza 1.! Istnieje tęczowy podciąg arytmetyczny ");

                    var subsequence = subsequences[T.Select((x, index) => (x, index)).First(x => x.x[c] == k).index];
                    foreach (var element in subsequence)
                    {
                        Console.ForegroundColor = (ConsoleColor)numbers[element - 1];
                        if ((ConsoleColor)numbers[element - 1] == ConsoleColor.DarkMagenta)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        Console.Write($"{element} ");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }

                return MakeMoveResult.Player1Won;
            }
            if (Player2Won())
            {
                if (demo)
                    Console.Write("Wygrana gracza 2.! Brak tęczowego podciagu arytmetycznego.");

                return MakeMoveResult.Player2Won;
            }

            return MakeMoveResult.NoOneWon;
        }

        private void Update(int number, int color)
        {
            numbers[number] = color;
            for (var i = 0; i < subsequences.Count; i++)
            {
                if (!subsequences[i].Contains(number + 1))
                    continue;

                if (T[i][color - 1] == 1)
                {
                    T.RemoveAt(i);
                    subsequences.RemoveAt(i);
                    i--;
                }
                else
                {
                    T[i][color - 1] = 1;
                    T[i][c]++;
                }
            }
        }

        private void DisplayMove(int player, int number, int color)
        {
            Console.Write($"Gracz {player} pokolorował liczbę {number + 1} na kolor ");
            Console.ForegroundColor = (ConsoleColor)color+1;
            if ((ConsoleColor)color == ConsoleColor.DarkMagenta)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            Console.Write($"{color}\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void DisplayState()
        {
            for (var i = 0; i < n; i++)
            {
                if (numbers[i] == 0)
                    Console.Write($"{i + 1} ");
                else
                {
                    Console.ForegroundColor = (ConsoleColor)numbers[i];
                    if ((ConsoleColor)numbers[i] == ConsoleColor.DarkMagenta)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write($"{i + 1} ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            Console.WriteLine();
        }

        private bool Player1Won() => T.Any(x => x[c] == k);
        private bool Player2Won() => T.Count == 0;
    }
}
