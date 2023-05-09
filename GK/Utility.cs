using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK
{
    public static class Utility
    {
        public static List<int[]> GetAllSubsequences(int n, int k)
        {
            var subsequences = new List<int[]>();
            var r = n; // różnica
            for (var i = r; i > 0; i--)
            {
                if (1 + (k - 1) * r <= n)
                {
                    var start = 1;
                    while (start + (k - 1) * r <= n)
                    {
                        var array = new int[k];
                        for (var j = 0; j < k; j++)
                            array[j] = start + j * r;

                        start++;
                        subsequences.Add(array);
                    }
                }
                r--;
            }

            return subsequences;
        }
    }

    public static class ConsoleExtension
    {
        public static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            for (var i = 0; i < Console.WindowWidth; i++)
                Console.Write(' ');
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
}
