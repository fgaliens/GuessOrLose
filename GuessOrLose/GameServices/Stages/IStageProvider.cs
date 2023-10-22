namespace GuessOrLose.GameServices.Stages
{
    public interface IStageProvider<T> where T : IGameStage
    {
        ValueTask<bool> IsStageAvailable();
        ValueTask<T> GetActiveStageAsync();
    }
}
