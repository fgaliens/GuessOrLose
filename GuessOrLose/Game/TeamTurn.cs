using GuessOrLose.Models;
using System.Reactive.Subjects;

namespace GuessOrLose.Game
{
    public class TeamTurn
    {
        public TeamTurn(Team team)
        {
            Team = team;
        }

        public Team Team { get; }
    }

    public interface INotifier<T> : IObserver<T>, ISubject<T>
    {
        void IObserver<T>.OnError(Exception e)
        { }

        void IObserver<T>.OnCompleted()
        { }
    }
}
