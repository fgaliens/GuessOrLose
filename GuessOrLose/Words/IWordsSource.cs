using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessOrLose.Words
{
    public interface IWordsSource
    {
        IAsyncEnumerable<Word> GetRandomWordsAsync(WordFilter filter);
    }
}
