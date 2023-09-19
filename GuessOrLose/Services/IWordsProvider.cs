using GuessOrLose.Models;

namespace GuessOrLose.Services
{
    public interface IWordsProvider
    {
        IAsyncEnumerable<Word> GetWordsAsync();
    }
}
