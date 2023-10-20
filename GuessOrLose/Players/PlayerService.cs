using GuessOrLose.Exceptions;
using GuessOrLose.Game;

namespace GuessOrLose.Players
{
    public class PlayerService : IPlayerService
    {
        private readonly IEqualityComparer<IGamePipeline> _gameEqualityComparer;
        private readonly Dictionary<Player, IGamePipeline> _playerToGameDict;
        private readonly Dictionary<IGamePipeline, List<Player>> _gameToPlayersDict;
        private readonly object _sync = new();

        public PlayerService(
            IEqualityComparer<Player> playerEqualityComparer,
            IEqualityComparer<IGamePipeline> gameEqualityComparer)
        {
            _playerToGameDict = new Dictionary<Player, IGamePipeline>(playerEqualityComparer);
            _gameToPlayersDict = new Dictionary<IGamePipeline, List<Player>>(gameEqualityComparer);
            _gameEqualityComparer = gameEqualityComparer;
        }

        public Task AddToGameAsync(Player player, IGamePipeline game)
        {
            lock (_sync)
            {
                var playerAdded = _playerToGameDict.TryAdd(player, game);
                if (!playerAdded)
                {
                    return Task.FromException(new ActionForbiddenException(ExceptionCode.PlayerNotInThisGame,
                        $"Player is participant of the other game"));
                }

                _gameToPlayersDict.TryAdd(game, new List<Player>());
                var players = _gameToPlayersDict[game];
                if (!players.Contains(player))
                {
                    players.Add(player);
                }

                return Task.CompletedTask;
            }
        }

        public Task<int> CountPlayersInGameAsync(IGamePipeline game)
        {
            lock (_sync)
            {
                if (_gameToPlayersDict.TryGetValue(game, out var players))
                {
                    return Task.FromResult(players.Count);
                }
            }

            return Task.FromResult(0);
        }

        public Task<IGamePipeline> GetGameByPlayerAsync(Player player)
        {
            lock (_sync)
            {
                if (_playerToGameDict.TryGetValue(player, out var game))
                {
                    return Task.FromResult(game);
                }
            }

            return Task.FromException<IGamePipeline>(
                new ObjectNotFoundException(ExceptionCode.GameNotFound, player, nameof(GetGameByPlayerAsync)));
        }

#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        public async IAsyncEnumerable<Player> GetPlayersByGameAsync(IGamePipeline game)
        {
            lock (_sync)
            {
                if (_gameToPlayersDict.TryGetValue(game, out var players))
                {
                    var playersToReturn = players.ToArray();
                    foreach (var player in playersToReturn)
                    {
                        yield return player;
                    }
                }
            }
        }
#pragma warning restore CS1998 

        public Task<bool> IsPlayerInGameAsync(Player player, IGamePipeline game)
        {
            lock (_sync)
            {
                if (_playerToGameDict.TryGetValue(player, out var storedGame))
                {
                    var equal = _gameEqualityComparer.Equals(storedGame, game);
                    return Task.FromResult(equal);
                }
            }

            return Task.FromResult(false);
        }

        public Task RemoveFromGameAsync(Player player)
        {
            lock (_sync)
            {
                if (_playerToGameDict.TryGetValue(player, out var game))
                {
                    _playerToGameDict.Remove(player);

                    if (_gameToPlayersDict.TryGetValue(game, out var players))
                    {
                        players.Remove(player);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
