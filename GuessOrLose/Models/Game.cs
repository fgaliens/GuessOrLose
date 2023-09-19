using GuessOrLose.Exceptions;
using System.Collections.Immutable;

namespace GuessOrLose.Models
{
    public enum GameState
    {
        Prepearing,
        Started,
        Finished
    }

    public class GamePreparation
    {
        private readonly object mutex = new();
        private ImmutableHashSet<Player> players = ImmutableHashSet<Player>.Empty;

        public IEnumerable<Player> Players => players;

        public Task JoinGameAsync(Player player)
        {
            lock (players)
            {
                players = players.Add(player);
            }
            return Task.CompletedTask;
        }

        public Task ExitGameAsync(Player player)
        {
            lock (mutex)
            {
                if (players.Contains(player))
                {
                    players = players.Remove(player);
                }
                else
                {
                    throw new IncorrectOperationException("Can't exit game because player is not a participant");
                }
            }
            return Task.CompletedTask;
        }
    }

    public class Game
    {
        private readonly object mutex = new();

        public Game(IInitiatedGame source)
        {
            if (source is null || source.Players is null)
            {
                throw new CreationException($"Unable to create game: amount of players must be equal or grater than 4 (but it's {count})");
            }

            var count = source.Players.Count();

            if (count >= 4)
            {
                throw new CreationException($"Unable to create game: amount of players must be equal or grater than 4 (but it's {count})");
            }

            if (count % 2 == 0)
            {
                throw new CreationException("Unable to create game: amount of players must be even");
            }

            //Players = source.Players;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public GameState State { get; private set; }

        public Task SetPlayerReadyAsync(Player player)
        {
            return Task.CompletedTask;
        }

        public Task SetPlayerNotReadyAsync(Player player)
        {
            return Task.CompletedTask;
        }

    }

    public class InitiatedGame : IInitiatedGame
    {
        private readonly HashSet<Player> players = new();

        public InitiatedGame(Player initiator)
        {
            Initiator = initiator ?? throw new ArgumentNullException(nameof(initiator));
            players.Add(initiator);
        }

        public IEnumerable<Player> Players => players.ToArray();
        public Guid Id { get; } = Guid.NewGuid();
        public Player Initiator { get; }

        public Task JoinGameAsync(Player player)
        {
            lock (players)
            {
                players.Add(player);
            }
            return Task.CompletedTask;
        }

        public Task ExitGameAsync(Player player)
        {
            lock (players)
            {
                if (players.Contains(player))
                {
                    players.Remove(player);
                }
                else
                {
                    throw new IncorrectOperationException("Can't exit game because player is not a participant");
                }
            }
            return Task.CompletedTask;
        }
    }
}
