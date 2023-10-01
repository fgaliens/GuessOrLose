using GuessOrLose.Data.Models;

namespace GuessOrLose.Data
{
    public interface IWordsSource
    {
        IAsyncEnumerable<WordDto> GetRandomWordsAsync(WordFilter filter);
    }
}
