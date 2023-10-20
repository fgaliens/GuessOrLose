using GuessOrLose.Exceptions;
using GuessOrLose.Players;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace GuessOrLose.WebApi.Storages
{
    public class PlayersStorage : IPlayersStorage
    {
        private readonly Dictionary<Guid, Player> _players;
        private readonly HashSet<string> _playerNames;
        private readonly Regex _playerNameValidator = new(@"^[-\w\d]+$");
        private readonly object _sync = new();

        public PlayersStorage()
        {
            _players = new Dictionary<Guid, Player>();
            _playerNames = new HashSet<string>(new PlayerNameComparer());
        }

        public Guid CreatePlayer(string playerName)
        {
            if (!_playerNameValidator.IsMatch(playerName))
            {
                throw new ValidationException(ExceptionCode.IncorrectName, $"Name '{playerName}' doesn`t match pattern")
                {
                    Detail = $"Name '{playerName}' is incorrect. It should contains "
                };
            }

            const int minimumVarity = 3;
            var charsVariety = playerName.ToHashSet().Count;

            if (charsVariety < minimumVarity)
            {
                throw new ValidationException(ExceptionCode.IncorrectName, $"Name '{playerName}' has low variety ({charsVariety})")
                {
                    Detail = $"Name '{playerName}' is incorrect. It should contains more than {minimumVarity} symbols"
                };
            }

            lock (_sync)
            {
                if (_playerNames.Contains(playerName))
                {
                    throw new ValidationException(ExceptionCode.IncorrectName, $"Player with name '{playerName}' logged in alredy")
                    {
                        Detail = $"Player with name '{playerName}' logged in alredy"
                    };
                }
                _playerNames.Add(playerName);

                var id = Guid.NewGuid();
                var player = new Player
                {
                    Id = id,
                    Name = playerName
                };

                var added = _players.TryAdd(id, player);
                if (!added)
                {
                    throw new InvalidOperationException($"Player with id {id} exist already in storage");
                }

                return id;
            }
        }

        public Player GetPlayer(Guid id)
        {
            lock (_sync)
            {
                if (_players.TryGetValue(id, out var player))
                {
                    return player;
                }
            }

            throw new ObjectNotFoundException(ExceptionCode.PlayerNotFound, id, nameof(PlayersStorage));
        }

        public bool RemovePlayer(Guid id)
        {
            lock ( _sync)
            {
                if ( _players.TryGetValue(id, out var player))
                {
                    _players.Remove(id);
                    _playerNames.Remove(player.Name);
                    return true;
                }
            }

            return false;
        }

        private class PlayerNameComparer : IEqualityComparer<string>
        {
            public bool Equals(string? x, string? y)
            {
                return string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
            }

            public int GetHashCode([DisallowNull] string obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
