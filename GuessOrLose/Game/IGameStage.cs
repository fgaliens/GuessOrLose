namespace GuessOrLose.Game
{
    public interface IGameStage
    {
        StageState State { get; }
        Task StartAsync(IGamePipeline game);
        IEnumerable<IGameStage> Substages { get; }
    }
}
