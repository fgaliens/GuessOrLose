using GuessOrLose.Players;

namespace GuessOrLose.WebApi.Storages
{
    public interface IPlayersStorage
    {
        Guid CreatePlayer(string playerName);
        Player GetPlayer(Guid id);
        bool RemovePlayer(Guid id);
    }
}
