namespace GuessOrLose.Data
{
    public interface ISingleValueStorage<TKey, TValue> : IValueStorage<TKey, TValue>
        where TKey : notnull
    {
        Task<bool> AddAsync(TKey key, TValue value, bool replace);
        AwaitableValueContainer<TValue> GetValueAsync(TKey key);
    }
}
