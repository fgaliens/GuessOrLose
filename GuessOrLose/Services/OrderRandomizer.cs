namespace GuessOrLose.Services
{
    public class OrderRandomizer : IOrderRandomizer
    {
        public void Randomize<T>(IList<T> list)
        {
            var count = list.Count;

            for (int i = 0; i < count; i++)
            {
                var newIndex = Random.Shared.Next(0, count);
                Swap(list, i, newIndex);
            }
        }

        private static void Swap<T>(IList<T> list, int from, int to)
        {
            if (from >= 0 && to < list.Count && from != to)
            {
                (list[to], list[from]) = (list[from], list[to]);
            }
        }
    }
}
