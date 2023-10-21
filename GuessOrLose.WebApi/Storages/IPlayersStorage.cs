using GuessOrLose.Players;

namespace GuessOrLose.WebApi.Storages
{
    public interface IPlayersStorage
    {
        Task<Guid> CreatePlayerAsync(string playerName);
        Task<Player> GetPlayerAsync(Guid id);
        Task<bool> RemovePlayerAsync(Guid id);
    }
}
