namespace GuessOrLose.Messages
{
    public interface IMessageReader<T> where T : Message
    {
        IAsyncEnumerable<T> ReadAllMessagesAsync();
    }
}
