using GuessOrLose.Exceptions;
using GuessOrLose.Players;
using Nito.AsyncEx;

namespace GuessOrLose.GameServices.Stages
{
    // TODO подписка на изменение состояния игры
    public class StageProvider<T> : IStageProvider<T> where T : IGameStage
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IPlayerService _playerService;
        private AsyncLazy<IGame> _getGameLazy;

        public StageProvider(IPlayerProvider playerProvider, IPlayerService playerService)
        {
            _getGameLazy = new(async () => 
            {
                var player = await playerProvider.GetCurrentPlayerAsync();
                var game = await playerService.GetGameByPlayerAsync(player);
                return game;
            });

            _playerProvider = playerProvider;
            _playerService = playerService;
        }

        public async ValueTask<bool> IsStageAvailable()
        {
            var game = await _getGameLazy;
            return game.ActiveStage is T;
        }

        public async ValueTask<T> GetActiveStageAsync()
        {
            var game = await _getGameLazy;
            if (game.ActiveStage is T stage)
            {
                return stage;
            }

            throw new IncorrectOperationException(ExceptionCode.UnexpectedGameStage, $"Current game stage is not {typeof(T)}");
        }

        private async Task InitializeAsyncInternal()
        {
            var player = await _playerProvider.GetCurrentPlayerAsync();
            var game = await _playerService.GetGameByPlayerAsync(player);

            if (game.ActiveStage is T stage)
            {
                //Avaliable = true;
            }
        }
    }
}
