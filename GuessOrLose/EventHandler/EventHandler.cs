using System.Reactive.Subjects;

namespace GuessOrLose.EventHandler
{
    public class EventHandler<T> : IEventHandler<T>
    {
        private readonly Subject<T> _subject;

        public EventHandler()
        {
            _subject = new Subject<T>();
            Observer = _subject;
            Observable = _subject;
        }

        public IObserver<T> Observer { get; }

        public IObservable<T> Observable { get; }
    }
}
