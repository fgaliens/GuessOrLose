namespace GuessOrLose.EventHandler
{
    public interface IEventHandlerProvider<T> : IObserver<T>, IObservable<T>
    {
        IEventHandler<T> EventHandler { get; }

        void IObserver<T>.OnNext(T value)
        {
            EventHandler.Observer.OnNext(value);
        }

        void IObserver<T>.OnError(Exception error)
        {
            EventHandler.Observer.OnError(error);
        }

        void IObserver<T>.OnCompleted()
        {
            EventHandler.Observer.OnCompleted();
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            return EventHandler.Observable.Subscribe(observer);
        }
    }
}
