namespace GuessOrLose.Messages
{
    public interface IMessageWriter<T> where T : Message, new()
    {
        void Write(Action<T> fillMessage);
        Task WriteAsync(Action<T> fillMessage);
    }
}
