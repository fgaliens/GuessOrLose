namespace GuessOrLose.EventHandler
{
    public class EventHandlerProvider<T> : IEventHandlerProvider<T>
    {
        public EventHandlerProvider()
        {
            EventHandler = new EventHandler<T>();
        }

        public EventHandlerProvider(IEventHandler<T> eventHandler)
        {
            EventHandler = eventHandler;
        }

        public IEventHandler<T> EventHandler { get; }
    }
}
