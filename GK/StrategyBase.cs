using GK.Strategy;

namespace GK
{
    public abstract class StrategyBase : IStrategy
    {
        protected readonly int _n;
        protected readonly int _k;
        protected readonly int _c;

        protected Random Random { get; } = new();
        protected List<int[]> Subsequences { get; }
        protected List<int[]> T { get; }

        protected StrategyBase(int n, int k, int c)
        {
            _n = n;
            _k = k;
            _c = c;

            Subsequences = Utility.GetAllSubsequences(n, k);

            T = new List<int[]>();
            for (var i = 0; i < Subsequences.Count; i++)
                T.Add(new int[c + 1]);
        }

        public abstract (int number, int color) MakeMove(IReadOnlyList<int> numbers);
    }
}
