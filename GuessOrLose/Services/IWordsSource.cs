using GuessOrLose.Models;

namespace GuessOrLose.Services
{
    public interface IWordsSource
    {
        IAsyncEnumerable<Word> GetRandomWordsAsync(int count);
    }
}
