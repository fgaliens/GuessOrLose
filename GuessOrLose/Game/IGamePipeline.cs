namespace GuessOrLose.Game
{
    public interface IGamePipeline
    {
        public Guid Id { get; }
        IGameStage ActiveStage { get; }
        Task NotifyStageCompleteAsync(IGameStage stage);
    }
}
