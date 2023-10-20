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
            services.AddScoped(sp =>
            {
                var httpContext = sp.GetRequiredService<IHttpContextAccessor>()
                    .HttpContext;

                if (httpContext?.User is null)
                {
                    throw new InvalidOperationException("Can`t access to User field in HttpContext");
                }

                var storage = sp.GetRequiredService<IPlayersStorage>();

                var id = httpContext.User.GetPlayerId();
                return storage.GetPlayer(id);
            });

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
