namespace GuessOrLose.EventHandler
{
    public abstract class EventArgs<TSource>
    {
        public abstract TSource Source { get; }
    }

    public interface IObserverItem<T>
    {
        IObserver<T> Observer { get; }
    }

    public interface IObservableItem<T>
    {
        IObservable<T> Observable { get; }
    }

    public class MyClass : IObserver<int>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(int value)
        {
            throw new NotImplementedException();
        }
    }
}
