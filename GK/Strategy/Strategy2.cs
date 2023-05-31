namespace GK.Strategy
{
    public class Strategy2 : StrategyBase, IStrategy
    {
        private int[] _A { get; set; }

        public Strategy2(int n, int k, int c) : base(n, k, c)
        {
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
            var colors = new int[_c + 1];
            colors[0] = int.MinValue;

            var maxSubsequence = FindMaxSubsequence();
            for (var i = 0; i < Subsequences.Count; i++)
                if (T[i][_k] == maxSubsequence)
                    foreach (var element in Subsequences[i])
                        if (numbers[element - 1] != 0)
                            colors[numbers[element - 1]]++;

            var max = colors.Max();
            var indexes = colors.Select((x, index) => (x, index)).Where(x => x.x == max).Select(x => x.index).ToList();

            return indexes[Random.Next(indexes.Count)];
        }

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
                    T[i][_c]++;
                }
            }
        }

        protected void UpdateAArray(IReadOnlyList<int> numbers)
        {
            var max = FindMaxSubsequence();
            _A = new int[_n];

            for (var i = 0; i < Subsequences.Count; i++)
            {
                if (T[i][_k] != max)
                    continue;

                for (var j = 0; j < Subsequences[i].Length; j++)
                {
                    if (numbers[Subsequences[i][j] - 1] != 0)
                        _A[Subsequences[i][j] - 1] = -1;
                    else
                        _A[Subsequences[i][j] - 1]++;
                }
            }

            for (var i = 0; i < _A.Length; i++)
                if (numbers[i] != 0)
                    _A[i] = -1;
        }

        protected int FindMaxSubsequence() => T.Select(t => t[_k]).Max();

        protected int GetNumberToColor()
        {
            var max = _A.Max();
            var maxIndexes = _A.Select((x, index) => (x, index)).Where(x => x.x == max).Select(x => x.index).ToList();
            var maxIndex = Random.Next(0, maxIndexes.Count);

            return maxIndexes[maxIndex];
        }
    }
}
