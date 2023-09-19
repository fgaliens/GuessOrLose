namespace GuessOrLose.Services
{
    public interface IOrderRandomizer
    {
        void Randomize<T>(IList<T> list);
    }
}
