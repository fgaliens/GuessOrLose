using GuessOrLose.WebApi.Services;
using GuessOrLose.WebApi.Storages;
using System.Security.Claims;

namespace GuessOrLose.WebApi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IPlayersStorage, PlayersStorage>();
            services.AddScoped<IPlayerProvider, PlayerProvider>();

            return services;
        }
    }

    public static class ClaimsPrincipalExtensions 
    {
        public static Guid GetPlayerId(this ClaimsPrincipal claimsPrincipal)
        {
            var idValue = claimsPrincipal.FindFirstValue(ClaimTypes.Id);
            if (Guid.TryParse(idValue, out var id))
            {
                return id;
            }

            throw new InvalidOperationException($"Can`t get claim of type '{ClaimTypes.Id}'");
        }
    }
}
