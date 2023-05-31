namespace GK.Strategy
{
    public class Strategy1 : StrategyBase, IStrategy
    {
        public Strategy1(int n, int k, int c) : base(n, k, c)
        {
        }

        public override (int number, int color) MakeMove(IReadOnlyList<int> numbers)
        {
            var maxColoredCount = T.Select(x => x[_c]).Max();
            var maxColoredCountSubsequenceIndexes = T.Select((x, index) => (x, index)).Where(x => x.x[_c] == maxColoredCount).Select(x => x.index).ToList();
            var maxColoredIndex = Random.Next(0, maxColoredCountSubsequenceIndexes.Count);

            var (number, color) = GetFringeElement(numbers, maxColoredCountSubsequenceIndexes[maxColoredIndex]);

            Update(number, color);
            return (number, color);
        }

        private (int number, int color) GetFringeElement(IReadOnlyList<int> numbers, int subsequenceIndex)
        {
            var fringeIndex = _k - 1;
            var color = GetRandomColor(subsequenceIndex);
            if (color == -1)
                return (-1, -1);

            var subsequence = Subsequences[subsequenceIndex];
            for (var i = 0; i < _k; i++)
            {
                if (numbers[subsequence[i] - 1] == 0)
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
}
