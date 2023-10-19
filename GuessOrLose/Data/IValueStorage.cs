namespace GuessOrLose.Data
{
    public interface IValueStorage<TKey, TValue>
        where TKey : notnull
    {
        Task<bool> ContainsKeyAsync(TKey key);
        Task<bool> AddAsync(TKey key, TValue value);
        Task<bool> RemoveKeyAsync(TKey key);
    }
}
