using System.Reactive.Subjects;

namespace GuessOrLose.EventHandler
{
    public class EventHandler<T> : IEventHandler<T>, IObserver<T>
    {
        private readonly Subject<T> _subject;
        private readonly List<Action<T>> _onNextCallbacks = new();
        private readonly List<Action<Exception>> _onErrorCallbacks = new();
        private readonly List<Action> _onCompletedCallbacks = new();

        public EventHandler()
        {
            _subject = new Subject<T>();
            //_subject.Subscribe(this);
            Observer = _subject;
            Observable = _subject;
        }

        public IObserver<T> Observer { get; }

        public IObservable<T> Observable { get; }

        public void AddCallback(Action<T> onNextCallback)
        {
            _onNextCallbacks.Add(onNextCallback);
        }

        public void AddCallback(Action<Exception> onErrorCallback)
        {
            _onErrorCallbacks.Add(onErrorCallback);
        }

        public void AddCallback(Action onCompletedCallback)
        {
            _onCompletedCallbacks.Add(onCompletedCallback);
        }

        public void OnCompleted()
        {
            foreach (var callback in _onCompletedCallbacks)
            {
                callback();
            }
            _subject.OnCompleted();
        }

        public void OnError(Exception error)
        {
            foreach (var callback in _onErrorCallbacks)
            {
                callback(error);
            }
            _subject.OnError(error);
        }

        public void OnNext(T value)
        {
            foreach (var callback in _onNextCallbacks)
            {
                callback(value);
            }
            _subject.OnNext(value);
        }
    }
}
