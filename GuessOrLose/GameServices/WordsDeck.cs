using GuessOrLose.Models;
using GuessOrLose.Services;

namespace GuessOrLose.GameServices
{
    public class WordsDeck
    {
        private bool _hasPeeked = false;
        private readonly object _sync = new();
        private readonly List<Word> _unusedWords = new();
        private readonly List<Word> _usedWords = new();
        private readonly IOrderRandomizer _orderRandomizer;

        public WordsDeck(IEnumerable<Word> words, IOrderRandomizer orderRandomizer)
        {
            _unusedWords.AddRange(words);
            _orderRandomizer = orderRandomizer;

            orderRandomizer.Randomize(_unusedWords);
        }

        public bool IsEmpty
        {
            get
            {
                lock (_sync)
                {
                    return _unusedWords.Count == 0;
                }
            }
        }

        public PeekedWord PeekWord()
        {
            lock (_sync)
            {
                if (_hasPeeked)
                {
                    return PeekedWord.Empty;
                }

                if (_unusedWords.Count > 0)
                {
                    _hasPeeked = true;

                    var lastWord = _unusedWords[^1];

                    return new PeekedWord(this, lastWord);

                }

                return PeekedWord.Empty;
            }
        }

        private bool HasPeeked
        {
            get
            {
                lock (_sync)
                {
                    return _hasPeeked;
                }
            }

            set 
            {
                lock (_sync)
                {
                    _hasPeeked = value;
                }
            }
        }

        private void RandomizeUnusedWords()
        {
            lock (_sync)
            {
                _orderRandomizer.Randomize(_unusedWords);
            }
        }

        private bool PopWord(Word word)
        {
            lock (_sync)
            {
                if (_unusedWords.Remove(word))
                {
                    _usedWords.Add(word);
                    return true;
                }
            }
            return false;
        }

        public readonly struct PeekedWord : IDisposable
        {
            private readonly Word? _word;
            private readonly WordsDeck? _deck;

            public static PeekedWord Empty { get; } = new PeekedWord();

            public PeekedWord(WordsDeck? wordsDeck, Word? word)
            {
                _deck = wordsDeck;
                _word = word;
            }

            public Word Word => HasValue ? _word! : throw new InvalidOperationException("There is no peeked word");

            public bool HasValue => _deck != null && _word != null;

            public void MarkAsUsed()
            {
                _deck?.PopWord(Word);
            }

            public void Skip()
            {
                _deck?.RandomizeUnusedWords();
            }

            public void Dispose()
            {
                if (!HasValue)
                    return;
                
                _deck!.HasPeeked = false;
                
            }
        }
    }
}
