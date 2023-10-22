using GuessOrLose.Data.Models;
using GuessOrLose.Words;

namespace GuessOrLose.Data
{
    public interface IWordsSource
    {
        IAsyncEnumerable<WordDto> GetRandomWordsAsync(WordFilter filter);
    }
}
