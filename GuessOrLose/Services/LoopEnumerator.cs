using GuessOrLose.Models;
using System.Collections.Generic;

namespace GuessOrLose.Services
{
    public class LoopEnumerator<T>
    {
        private readonly IEnumerator<T> loopedEnumerator;
        private readonly IEnumerable<T> source;

        public LoopEnumerator(IEnumerable<T> source)
        {
            loopedEnumerator = GetLoopedEnumerable().GetEnumerator();
            this.source = source;
        }

        public T Next()
        {
            loopedEnumerator.MoveNext();
            return loopedEnumerator.Current;
        }

        private IEnumerable<T> GetLoopedEnumerable()
        {
            while (true)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
        }
    }

    public class WordsDeck
    {
        private readonly IOrderRandomizer orderRandomizer;
        private readonly List<Word> unusedWords = new();
        private readonly List<Word> usedWords = new();

        public WordsDeck(IEnumerable<Word> words, IOrderRandomizer orderRandomizer) 
        {
            unusedWords.AddRange(words);
            this.orderRandomizer = orderRandomizer;
        }

        public bool TryPeekWord(out Word word)
        {
            if (unusedWords.Count > 0)
            {
                word = unusedWords[^1];
                unusedWords.Remove(word);
                return true;
            }

            word = null!;
            return false;
        }

        
    }
}
