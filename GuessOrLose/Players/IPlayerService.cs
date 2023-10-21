using GuessOrLose.GameServices;

namespace GuessOrLose.Players
{
    public interface IPlayerService
    {
        Task AddToGameAsync(Player player, IGame game);
        Task<bool> IsPlayerInGameAsync(Player player, IGame game);
        Task<bool> IsPlayerInGameAsync(Player player);
        Task<IGame> GetGameByPlayerAsync(Player player);
        IAsyncEnumerable<Player> GetPlayersByGameAsync(IGame game);
        Task<int> CountPlayersInGameAsync(IGame game);
        Task RemoveFromGameAsync(Player player);
    }
}
