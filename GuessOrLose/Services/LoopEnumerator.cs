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
}
