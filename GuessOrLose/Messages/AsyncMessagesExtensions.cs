using System.Threading.Channels;

namespace GuessOrLose.Messages
{
    public static class AsyncMessagesExtensions
    {
        public static Builder AddAsyncMessages(this IServiceCollection services)
        {
            return new Builder(services);
        }

        public class Builder
        {
            private readonly IServiceCollection _services;

            public Builder(IServiceCollection services)
            {
                _services = services;
            }

            public Builder AddHandlerFor<TMessage>() where TMessage : Message, new()
            {
                _services.AddSingleton(sp => Channel.CreateUnbounded<MessageBody<TMessage>>(new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = false,
                    AllowSynchronousContinuations = false
                }));

                _services.AddSingleton(sp =>
                {
                    var channel = sp.GetRequiredService<Channel<MessageBody<TMessage>>>();
                    return channel.Reader;
                });

                _services.AddSingleton(sp =>
                {
                    var channel = sp.GetRequiredService<Channel<MessageBody<TMessage>>>();
                    return channel.Writer;
                });

                _services.AddSingleton<IMessageWriter<TMessage>, MessageWriter<TMessage>>();
                _services.AddSingleton<IMessageReader<TMessage>, MessageReader<TMessage>>();

                return this;
            }

            public Builder AddHandlerFor<TMessage, THandler>() 
                where TMessage : Message, new()
                where THandler : MessageHandler<TMessage>
            {
                AddHandlerFor<TMessage>();
                _services.AddSingleton<MessageHandler<TMessage>, THandler>();

                return this;
            }

            public IServiceCollection Build()
            {
                return _services;
            }
        }
    }
}
