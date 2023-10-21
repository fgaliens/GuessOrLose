using GuessOrLose.Players;

namespace GuessOrLose.WebApi.Services
{
    public interface IPlayerProvider
    {
        Task<Player> GetCurrentPlayerAsync();
    }
}
