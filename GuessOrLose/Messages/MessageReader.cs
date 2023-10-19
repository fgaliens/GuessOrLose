using System.Threading.Channels;

namespace GuessOrLose.Messages
{
    public class MessageReader<T> : IMessageReader<T> where T : Message
    {
        private readonly ChannelReader<MessageBody<T>> _channelReader;

        public MessageReader(ChannelReader<MessageBody<T>> channelReader)
        {
            _channelReader = channelReader;
        }

        public async IAsyncEnumerable<T> ReadAllMessagesAsync()
        {
            var messages = _channelReader.ReadAllAsync();

            await foreach (var message in messages)
            {
                message.ResponsePropagator?.SetResult();
                yield return message.Value;
            }
        }
    }
}
