using GuessOrLose.Data;
using GuessOrLose.Exceptions;
using GuessOrLose.Models;
using Nito.AsyncEx;

namespace GuessOrLose.Services
{
    public interface IGameProvider
    {
        Task<Game> GetCurrentGameAsync();
    }

    public interface ICurrentPlayerService
    {
        Task<Player> GetCurrentPlayerAsync();
        Task<Team> GetCurrentTeamAsync();
    }

    public interface IPlayerService
    {
        IAsyncEnumerable<Player> GetPlayersAsync();
        IAsyncEnumerable<Team> GetTeamsAsync();
    }

    public class TeamTurn
    {
        public TeamTurn(Team team)
        {
            
        }
    }

    public class Round
    {
        private readonly Game game;

        public bool Finished { get; private set; }

        public Round(Game game)
        {
            this.game = game;
        }

        public TeamTurn NextTurn()
        {
            return new 
        }
    }

    public class Game
    {
        private readonly Stack<Round> rounds = new();
        private readonly IEnumerable<Player> players;
        private readonly LoopEnumerator<Team> teams;
        private readonly IEnumerable<Word> words;

        public Game(IEnumerable<Player> players, ITeamsBuilder teamsBuilder, IEnumerable<Word> words)
        {
            this.players = players;
            teams = new LoopEnumerator<Team>(teamsBuilder.BuildTeams(players));
            
            this.words = words;
        }

        public LoopEnumerator<Team> Teams => teams;

        public Round StartRound()
        {
            if (rounds.TryPeek(out var lastRound))
            {
                if (!lastRound.Finished)
                {
                    throw new IncorrectOperationException("");
                }
            }

            if (rounds.Count == 3)
            {
                throw new IncorrectOperationException("");
            }

            return new(this);
        }
    }
}
