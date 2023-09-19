using GuessOrLose.Data;
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

    public class Game
    {
        private readonly object _sync = new object();

        private readonly Stack<GameRound> _rounds = new();
        private readonly IPlayerService playerService;
        private readonly IWordsProvider words;

        public Game(IPlayerService playerService, IWordsProvider words)
        {
            this.playerService = playerService;
            this.words = words;
        }

        public Task<RoundTurn> CreateNewRound()
        {

        }

        //public Task<IEnumerable<Player>> GetPlayersAsync()
        //{
        //    return Task.FromResult(_players.AsEnumerable());
        //}
    }

    public abstract class GameRound
    {
        private readonly object _sync = new object();
        private readonly Queue<Word> _unexplainedWords = new();
        private readonly Queue<Word> _explainedWords = new();

        public GameRound(Game game)
        {
        }

        public abstract bool WordsCanBeSkipped { get; }

        public Task<RoundTurn> StartTurn()
        {

        }

        public Task<bool> Skip()
        {
            if (!WordsCanBeSkipped)
            {
                return Task.FromResult(false);
            }


        }

        public Task<bool> Next()
        {

        }
    }
    
    public class RoundTurn
    {
        public RoundTurn(GameRound round, Team team)
        {
        }
    }
}
