using GuessOrLose.Exceptions;
using GuessOrLose.Players;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace GuessOrLose.GameServices
{
    public class GameService : IGameService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ConcurrentDictionary<Guid, IGame> _games = new();

        public GameService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<IGame> CreateGameAsync()
        {
            var game = serviceProvider.GetRequiredService<IGame>();
            var added = _games.TryAdd(game.Id, game);

            if (!added)
            {
                throw new InternalException(ExceptionCode.DuplicatedId, "Id of created game is duplicated")
                {
                    Detail = "GameServices creation error. Try again."
                };
            }

            await game.ActiveStage.StartAsync(game);

            return game;
        }

        public Task<IGame> FindGameById(Guid id)
        {
            if (_games.TryGetValue(id, out var game))
            {
                return Task.FromResult(game);
            }

            throw new ObjectNotFoundException(ExceptionCode.GameNotFound, id);
        }
    }
}
