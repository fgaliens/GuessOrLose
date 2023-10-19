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

            public Builder AddHandlerFor<T>()
            {
                _services.AddSingleton(sp => Channel.CreateUnbounded<MessageBody<T>>(new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = false,
                    AllowSynchronousContinuations = false
                }));

                _services.AddSingleton(sp =>
                {
                    var channel = sp.GetRequiredService<Channel<MessageBody<T>>>();
                    return channel.Reader;
                });

                _services.AddSingleton(sp =>
                {
                    var channel = sp.GetRequiredService<Channel<MessageBody<T>>>();
                    return channel.Writer;
                });

                _services.AddSingleton<IMessageWriter<T>, MessageWriter<T>>();
                _services.AddSingleton<IMessageReader<T>, MessageReader<T>>();

                return this;
            }

            public IServiceCollection Build()
            {
                return _services;
            }
        }
    }
}
