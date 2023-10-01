using GuessOrLose.Models;

namespace GuessOrLose.Game
{
    public interface IWordsProvider
    {
        IAsyncEnumerable<Word> GetWordsAsync();
    }
}
