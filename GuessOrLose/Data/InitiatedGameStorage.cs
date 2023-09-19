using GuessOrLose.Exceptions;
using GuessOrLose.Models;
using System.Collections.Concurrent;

namespace GuessOrLose.Data
{
    public class GameStorage
    {
        private readonly ConcurrentDictionary<Guid, IGame> games = new();

        public Task AddAsync(IGame game)
        {
            if (game is null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            var success = games.TryAdd(game.Id, game);

            if (!success)
            {
                throw new IncorrectOperationException("Game with such ID has been added already");
            }

            return Task.CompletedTask;
        }

        public Task<IGame> GetByIdAsync(Guid id)
        {
            if (games.TryGetValue(id, out var game))
            {
                return Task.FromResult(game!);
            }

            throw new NotFoundException("No initiated game with such ID");
        }

        public Task RemoveAsync(IGame game)
        {
            if (!games.TryRemove(game.Id, out _))
            {
                throw new NotFoundException("No initiated game with such ID");
            }

            return Task.CompletedTask;
        }
    }

    public class InitiatedGameStorage
    {
        private readonly ConcurrentDictionary<Guid, IInitiatedGame> games = new();

        public Task AddAsync(IInitiatedGame game)
        {
            if (game is null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            var success = games.TryAdd(game.Id, game);
            
            if (!success)
            {
                throw new IncorrectOperationException("Game with such ID has been added already");
            }

            return Task.CompletedTask;
        }

        public Task<IInitiatedGame> GetByIdAsync(Guid id)
        {
            if (games.TryGetValue(id, out var game))
            {
                return Task.FromResult(game!);
            }

            throw new NotFoundException("No initiated game with such ID");
        }

        public Task RemoveAsync(IInitiatedGame game)
        {
            if (!games.TryRemove(game.Id, out _))
            {
                throw new NotFoundException("No initiated game with such ID");
            }

            return Task.CompletedTask;
        }
    }
}
