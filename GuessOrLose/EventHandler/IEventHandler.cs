namespace GuessOrLose.EventHandler
{
    public interface IEventHandler<T>
    {
        IObserver<T> Observer { get; }
        IObservable<T> Observable { get; }

        void AddCallback(Action<T> onNextCallback);

        void AddCallback(Action<Exception> onErrorCallback);

        void AddCallback(Action onCompletedCallback);
        
    }
}
