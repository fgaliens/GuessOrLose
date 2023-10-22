using GuessOrLose.WebApi.Services;
using GuessOrLose.WebApi.Storages;

namespace GuessOrLose.WebApi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IPlayersStorage, PlayersStorage>();

            return services;
        }
    }
}
