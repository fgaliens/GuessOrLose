using GuessOrLose.Players;

namespace GuessOrLose.GameServices
{
    public interface IGameService
    {
        Task<IGame> CreateGameAsync();
        Task<IGame> FindGameById(Guid id);
    }
}
