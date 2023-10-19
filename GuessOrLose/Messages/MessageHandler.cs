namespace GuessOrLose.Messages
{
    public abstract class MessageHandler<T> where T : Message
    {
        private readonly IMessageReader<T> _messageReader;

        public MessageHandler(IMessageReader<T> messageReader)
        {
            _messageReader = messageReader;

            var loopDelegate = new Func<Task>(StartHandlingLoopAsync);
            HandlingLoopTask = Task.Run(loopDelegate);
        }

        public Task HandlingLoopTask { get; init; }

        protected abstract Task HandleMessageAsync(T message);

        protected virtual Task OnErrorAsync(Exception exception, T message)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnStartAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnCompleteAsync()
        {
            return Task.CompletedTask;
        }

        private async Task StartHandlingLoopAsync()
        {
            await OnStartAsync();

            var messages = _messageReader.ReadAllMessagesAsync();
            await foreach (var message in messages)
            {
                try
                {
                    await HandleMessageAsync(message);
                }
                catch (Exception ex)
                {
                    await OnErrorAsync(ex, message);
                }
            }

            await OnCompleteAsync();
        }
    }
}
