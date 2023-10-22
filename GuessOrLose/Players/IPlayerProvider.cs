namespace GuessOrLose.Players
{
    public interface IPlayerProvider
    {
        Task<Player> GetCurrentPlayerAsync();
    }
}
