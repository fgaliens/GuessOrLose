using GuessOrLose.Game;

namespace GuessOrLose.Players
{
    public interface IPlayerService
    {
        Task AddToGameAsync(Player player, IGamePipeline game);
        Task<bool> IsPlayerInGameAsync(Player player, IGamePipeline game);
        Task<IGamePipeline> GetGameByPlayerAsync(Player player);
        IAsyncEnumerable<Player> GetPlayersByGameAsync(IGamePipeline game);
        Task<int> CountPlayersInGameAsync(IGamePipeline game);
        Task RemoveFromGameAsync(Player player);
    }
}
