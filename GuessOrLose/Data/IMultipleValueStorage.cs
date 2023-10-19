namespace GuessOrLose.Data
{
    public interface IMultipleValueStorage<TKey, TValue> : IValueStorage<TKey, TValue>
        where TKey : notnull
    {
        Task<bool> RemoveValueAsync(TKey key, TValue value);
        IAsyncEnumerable<TValue> GetValuesAsync(TKey key);
    }
}
