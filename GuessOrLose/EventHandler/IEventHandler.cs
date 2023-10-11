using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace GuessOrLose.EventHandler
{
    public interface IEventHandler<T>
    {
        IObserver<T> Observer { get; }
        IObservable<T> Observable { get; }
    }
}
