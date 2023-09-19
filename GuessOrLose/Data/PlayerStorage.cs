using GuessOrLose.Exceptions;
using GuessOrLose.Models;
using System.Collections.Concurrent;

namespace GuessOrLose.Data
{
    public class PlayerStorage
    {
        private readonly ConcurrentDictionary<Guid, Player> players = new();

        public Task<Player> AddPlayerAsync(string name)
        {
            if (name is { Length: <= 3 } || string.IsNullOrWhiteSpace(name))
            {
                throw new CreationException("Unable to add player: invalid name");
            }

            var player = new Player(Guid.NewGuid())
            {
                Name = name
            };

            players.TryAdd(player.Id, player);

            return Task.FromResult(player);
        }

        public Task<Player> GetPlayerAsync(Guid playerId)
        {
            if (players.TryGetValue(playerId, out var player))
            {
                return Task.FromResult(player);
            }

            throw new NotFoundException($"Player with id '{playerId}' not found");
        }
    }
}
