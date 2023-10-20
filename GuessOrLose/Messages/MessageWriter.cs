using GuessOrLose.Exceptions;
using System.Threading.Channels;

namespace GuessOrLose.Messages
{
    public class MessageWriter<T> : IMessageWriter<T> where T : Message, new()
    {
        private readonly ChannelWriter<MessageBody<T>> _channelWriter;

        public MessageWriter(ChannelWriter<MessageBody<T>> channelWriter)
        {
            _channelWriter = channelWriter;
        }

        public void Write(Action<T> fillMessage)
        {
            var message = new T();
            fillMessage(message);
            if (!message.IsValid())
            {
                throw new ValidationException(ExceptionCode.MessageIsNotValid, "Message is not valid");
            }

            var messageBody = new MessageBody<T>
            {
                Value = message,
                ResponsePropagator = null
            };

            var written = _channelWriter.TryWrite(messageBody);

            if (!written)
            {
                throw new IncorrectOperationException(ExceptionCode.MessageWasNotWritten, "Message wasn't written to channel");
            }
        }

        public async Task WriteAsync(Action<T> fillMessage)
        {
            var message = new T();
            fillMessage(message);

            var messageBody = new MessageBody<T>
            {
                Value = message,
                ResponsePropagator = new()
            };

            await _channelWriter.WriteAsync(messageBody);

            await messageBody.ResponsePropagator.Task;
        }
    }
}
