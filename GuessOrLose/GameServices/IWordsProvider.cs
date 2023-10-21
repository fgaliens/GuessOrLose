using GuessOrLose.Models;

namespace GuessOrLose.GameServices
{
    public interface IWordsProvider
    {
        IAsyncEnumerable<Word> GetWordsAsync();
    }
}
