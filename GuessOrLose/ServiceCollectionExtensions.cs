using GuessOrLose.GameServices;
using GuessOrLose.GameServices.Stages;
using GuessOrLose.Helpers.EqualityComparers;
using GuessOrLose.Players;
using Microsoft.Extensions.DependencyInjection;

namespace GuessOrLose
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGuessOrLoseGame(this IServiceCollection services, Action<Configurer> configure)
        {
            var configurer = new Configurer(services);
            configure(configurer);

            #region GameServices

            services.AddSingleton<IGameService, GameService>();
            services.AddTransient<IGame, Game>();

            #endregion

            #region GameStages

            services.AddScoped(typeof(IStageProvider<>), typeof(StageProvider<>));
            services.AddTransient<JoinPlayersStage>();

            #endregion

            #region PlayerServices

            services.AddSingleton<IPlayerService, PlayerService>();

            #endregion

            #region Helpers

            services.AddSingleton(typeof(IEqualityComparer<>), typeof(IdentifiableObjectComparer<>));

            #endregion

            return services;
        }

        public class Configurer
        {
            public Configurer(IServiceCollection services)
            {
                PlayerServices = new(services);
            }

            public PlayerServicesBuilder PlayerServices { get; }
        }

        public abstract class Builder
        {
            public Builder(IServiceCollection services)
            {
                Services = services;
            }

            public IServiceCollection Services { get; }
        }

        public class PlayerServicesBuilder : Builder
        {
            public PlayerServicesBuilder(IServiceCollection services) : base(services)
            { }

            public PlayerServicesBuilder SetPlayerProvider<T>() where T : class, IPlayerProvider
            {
                Services.AddScoped<IPlayerProvider, T>();
                return this;
            }
        }
    }
}
