using GuessOrLose.Words;

namespace GuessOrLose.GameServices
{
    public interface IWordsProvider
    {
        IAsyncEnumerable<Word> GetWordsAsync();
    }
}
