namespace GuessOrLose.Words
{
    internal class FakeWordsSource : IWordsSource
    {
#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        public async IAsyncEnumerable<Word> GetRandomWordsAsync(WordFilter filter)
        {
            for (var i = 0; i < filter.Limit; i++)
            {
                yield return new Word
                {
                    Value = $"Слово {i + 1}",
                    Meaning = $"Значение слова {i + 1}"
                };
            }
        }
#pragma warning restore CS1998
    }
}
