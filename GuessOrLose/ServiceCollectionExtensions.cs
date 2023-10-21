using GuessOrLose.GameServices;
using GuessOrLose.GameServices.Stages;
using GuessOrLose.Helpers.EqualityComparers;
using GuessOrLose.Messages;
using GuessOrLose.Models.Messages;
using GuessOrLose.Players;
using Microsoft.Extensions.DependencyInjection;

namespace GuessOrLose
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGuessOrLoseGame(this IServiceCollection services)
        {
            #region GameServices
            
            services.AddSingleton<IGameService, GameService>();
            services.AddTransient<IGame, Game>();

            #endregion

            #region GameStages
            
            services.AddTransient<JoinPlayersStage>();

            #endregion

            #region PlayerServices

            services.AddSingleton<IPlayerService, PlayerService>();

            #endregion

            #region Messages

            services.AddAsyncMessages()
                .AddHandlerFor<JoinPlayersStageMessages.PlayerJoined>()
                .AddHandlerFor<JoinPlayersStageMessages.PlayerReadyToStart>()
                .AddHandlerFor<JoinPlayersStageMessages.StateChanged>();

            #endregion

            #region Helpers

            services.AddSingleton(typeof(IEqualityComparer<>), typeof(IdentifiableObjectComparer<>));

            #endregion

            return services;
        }
    }
}
