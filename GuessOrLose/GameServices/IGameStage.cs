namespace GuessOrLose.GameServices
{
    public interface IGameStage
    {
        StageState State { get; }
        Task StartAsync(IGame game);
        IEnumerable<IGameStage> Substages { get; }
    }
}
