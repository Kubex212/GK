using System.Collections.Generic;
using System.Linq;

namespace GK
{
    
    public class Strategy1Player2 : Strategy1Base
    {
        private readonly int k;
        private readonly int c;

        public Strategy1Player2(int n, int k, int c) : base(n, k, c)
        {
            this.k = k;
            this.c = c;
        }

        public override (int number, int color) MakeMove(IReadOnlyList<int> numbers)
        {
            var maxColoredCount = T.Select(x => x[c]).Max();
            var maxColoredCountSubsequenceIndexes = T.Select((x, index) => (x, index)).Where(x => x.x[c] == maxColoredCount).Select(x => x.index).ToList();
            var maxColoredIndex = Random.Next(0, maxColoredCountSubsequenceIndexes.Count);

            var (number, color) = GetFringeElement(numbers, maxColoredCountSubsequenceIndexes[maxColoredIndex]);

            Update(number, color);
            return (number, color);
        }

        private (int number, int color) GetFringeElement(IReadOnlyList<int> numbers, int subsequenceIndex)
        {
            var fringeIndex = k - 1;
            var color = GetRandomColor(subsequenceIndex);
            if (color == -1)
                return (-1, -1);

            var subsequence = Subsequences[subsequenceIndex];
            for (var i = 0; i < k; i++)
            {
                if (i < k && numbers[subsequence[i] - 1] == 0)
                    return (subsequence[i] - 1, color);

                if (fringeIndex - i >= 0 && numbers[subsequence[fringeIndex - i] - 1] == 0)
                    return (subsequence[fringeIndex - i] - 1, color);
            }

            // all numbers colored
            return (-1, -1);
        }

        /// <summary>
        /// Returns random color used in a given subsequence or 1 if no color used.
        /// </summary>
        /// <param name="subsequenceIndex">Index of a subsequence in which used colors will be searched.</param>
        private int GetRandomColor(int subsequenceIndex)
        {
            var usedColors = T[subsequenceIndex].Reverse().Skip(1).Where(x => x > 0).ToList();
            return usedColors.Count == 0 ? 1 : usedColors[Random.Next(0, usedColors.Count)];
        }
        
    }

    public abstract class Strategy1Base : IStrategy
    {
        private readonly int n;
        private readonly int k;
        private readonly int c;

        protected Random Random { get; } = new Random();
        protected List<int[]> Subsequences { get; private set; }
        protected List<int[]> T { get; private set; }

        protected Strategy1Base(int n, int k, int c)
        {
            this.n = n;
            this.k = k;
            this.c = c;

            Subsequences = Utility.GetAllSubsequences(n, k);

            T = new List<int[]>();
            for (var i = 0; i < Subsequences.Count; i++)
                T.Add(new int[c + 1]);
        }

        public virtual void Reset()
        {
            ResetBase();
        }

        public abstract (int number, int color) MakeMove(IReadOnlyList<int> numbers);

        public void Update(int number, int color)
        {
            for (var i = 0; i < Subsequences.Count; i++)
            {
                if (!Subsequences[i].Contains(number + 1))
                    continue;

                if (T[i][color - 1] == 1)
                {
                    T.RemoveAt(i);
                    Subsequences.RemoveAt(i);
                    i--;
                }
                else
                {
                    T[i][color - 1] = 1;
                    T[i][c]++;
                }
            }
        }

        protected void ResetBase()
        {
            Subsequences = Utility.GetAllSubsequences(n, k);

            T = new List<int[]>();
            for (var i = 0; i < Subsequences.Count; i++)
                T.Add(new int[c + 1]);
        }
    }

    public interface IStrategy
    {
        (int number, int color) MakeMove(IReadOnlyList<int> numbers);
        void Update(int number, int color);
        void Reset();
    }

    public class Strategy2Player2 : Strategy2Base
    {
        private readonly int k;
        private readonly int c;

        public Strategy2Player2(int n, int k, int c) : base(n, k, c)
        {
            this.k = k;
            this.c = c;
        }

        public override (int number, int color) MakeMove(IReadOnlyList<int> numbers)
        {
            UpdateAArray(numbers);

            var number = GetNumberToColor();
            var color = GetMostUsedColor(numbers);

            Update(number, color);
            return (number, color);
        }

        private int GetMostUsedColor(IReadOnlyList<int> numbers)
        {
            var colors = new int[c + 1];
            colors[0] = int.MinValue;

            var maxSubseqence = FindMaxSubsequence();
            for (var i = 0; i < Subsequences.Count; i++)
                if (T[i][k] == maxSubseqence)
                    foreach (var element in Subsequences[i])
                        if (numbers[element - 1] != 0)
                            colors[numbers[element - 1]]++;

            var max = colors.Max();
            var indexes = colors.Select((x, index) => (x, index)).Where(x => x.x == max).Select(x => x.index).ToList();

            return indexes[Random.Next(indexes.Count)];
        }
    }

    public abstract class Strategy2Base : IStrategy
    {
        private readonly int n;
        private readonly int k;
        private readonly int c;

        protected Random Random { get; } = new Random();
        protected List<int[]> Subsequences { get; private set; }
        protected List<int[]> T { get; private set; }
        protected int[] A { get; private set; }

        protected Strategy2Base(int n, int k, int c)
        {
            this.n = n;
            this.k = k;
            this.c = c;

            Subsequences = Utility.GetAllSubsequences(n, k);

            T = new List<int[]>();
            for (var i = 0; i < Subsequences.Count; i++)
                // T.Add(new int[k + 1]);
                T.Add(new int[c + 1]);
        }

        public void Reset()
        {
            Subsequences = Utility.GetAllSubsequences(n, k);

            T = new List<int[]>();
            for (var i = 0; i < Subsequences.Count; i++)
                // T.Add(new int[k + 1]);
                T.Add(new int[c + 1]);
        }

        public abstract (int number, int color) MakeMove(IReadOnlyList<int> numbers);

        public void Update(int number, int color)
        {

            for (var i = 0; i < Subsequences.Count; i++)
            {
                if (!Subsequences[i].Contains(number + 1))
                    continue;

                if (T[i][color - 1] == 1)
                {
                    T.RemoveAt(i);
                    Subsequences.RemoveAt(i);
                    i--;
                }
                else
                {
                    T[i][color - 1] = 1;
                    T[i][c]++;
                }
            }
        }

        protected void UpdateAArray(IReadOnlyList<int> numbers)
        {
            var max = FindMaxSubsequence();
            A = new int[n];

            for (var i = 0; i < Subsequences.Count; i++)
            {
                if (T[i][k] != max)
                    continue;

                for (var j = 0; j < Subsequences[i].Length; j++)
                {
                    if (numbers[Subsequences[i][j] - 1] != 0)
                        A[Subsequences[i][j] - 1] = -1;
                    else
                        A[Subsequences[i][j] - 1]++;
                }
            }

            for (var i = 0; i < A.Length; i++)
                if (numbers[i] != 0)
                    A[i] = -1;
        }

        protected int FindMaxSubsequence() => T.Select(t => t[k]).Max();

        protected int GetNumberToColor()
        {
            var max = A.Max();
            var maxIndexes = A.Select((x, index) => (x, index)).Where(x => x.x == max).Select(x => x.index).ToList();
            var maxIndex = Random.Next(0, maxIndexes.Count);

            return maxIndexes[maxIndex];
        }
    }
}
