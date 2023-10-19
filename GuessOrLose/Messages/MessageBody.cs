namespace GuessOrLose.Messages
{
    public record MessageBody<T>
    {
        public required T Value { get; init; }
        public TaskCompletionSource? ResponsePropagator { get; init; }
    }
}
