namespace GK.Strategy
{
    public interface IStrategy
    {
        (int number, int color) MakeMove(IReadOnlyList<int> numbers);
        void Update(int number, int color);
    }
}
