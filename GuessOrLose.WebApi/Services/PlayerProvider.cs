using GuessOrLose.Players;
using GuessOrLose.WebApi.Storages;

namespace GuessOrLose.WebApi.Services
{
    public class PlayerProvider : IPlayerProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPlayersStorage _playersStorage;

        public PlayerProvider(IHttpContextAccessor httpContextAccessor, IPlayersStorage playersStorage)
        {
            _httpContextAccessor = httpContextAccessor;
            _playersStorage = playersStorage;
        }

        public Task<Player> GetCurrentPlayerAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.User is null)
            {
                return Task.FromException<Player>(
                    new InvalidOperationException("Can`t access to User field in HttpContext"));
            }

            var id = httpContext.User.GetPlayerId();
            return _playersStorage.GetPlayerAsync(id);
        }
    }
}
